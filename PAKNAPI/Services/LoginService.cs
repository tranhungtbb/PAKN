using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.Results;
using PAKNAPI.Model.ModelAuth;
using PAKNAPI.Model;
using PAKNAPI.ModelBase;

namespace PAKNAPI.Services
{
    public class LoginService
    {
        private readonly IAppSetting _appSetting;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _context;

        public LoginService(IAppSetting appSetting, IConfiguration configuration, IHttpContextAccessor context)
        {
            this._appSetting = appSetting;
            _config = configuration;
            _context = context;
        }
        public async Task<object> AuthenticateAsync(LoginIN loginIN)
        {
            try
            {
                List<SYUSRLogin> user = await new SYUSRLogin(_appSetting).SYUSRLoginDAO(loginIN.UserName);

                if (user != null && user.Count > 0)
                {
                    if (user[0].TypeId == 1)
                    {
                        var unit = await new SYUnit(_appSetting).SYUnitGetByID(user[0].UnitId);
                        if (!unit.IsActived)
                        {
                            return new ResultApi
                            {
                                Success = ResultCode.ORROR,
                                Result = 0,
                                Message = "Đơn vị của bạn đang hết hiệu lực"
                            };
                        }
                    }

                    if (!(bool)user[0].IsActived)
                    {
                        return new ResultApi
                        {
                            Success = ResultCode.ORROR,
                            Result = 0,
                            Message = "Tài khoản của bạn đang hết hiệu lực"
                        };
                    }
                    PasswordHasher hasher = new PasswordHasher();
                    if (hasher.AuthenticateUser(loginIN.Password, user[0].Password, user[0].Salt))
                    {
                        List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId =
                            await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(Int32.Parse(user[0].Id.ToString()));
                        if (rsSYUSRGetPermissionByUserId != null && rsSYUSRGetPermissionByUserId.Count > 0)
                        {
                            BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(_context.HttpContext.Request);
                            //var s = Request.Headers;
                            SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
                            {
                                UserId = user[0].Id,
                                FullName = user[0].FullName,
                                Action = "Login",
                                IPAddress = baseRequest.ipAddress,
                                MACAddress = baseRequest.macAddress,
                                Description = baseRequest.logAction,
                                CreatedDate = DateTime.Now,
                                Status = 1,
                                Exception = null
                            };

                            LoginResponse response = await generateJwtToken(user[0]);

                            if (user[0].IsActived == false) { sYSystemLogInsertIN.Status = 0; };
                            await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
                            SYUserUserAgent query = new SYUserUserAgent(user[0].Id, response.AccessToken, _context.HttpContext.Request.Headers["User-Agent"].ToString(), baseRequest.ipAddress, true);
                            await new SYUserUserAgent(_appSetting).SYUserUserAgentInsertDAO(query);

                            response.Permissions = rsSYUSRGetPermissionByUserId[0].Permissions;
                            response.PermissionCategories = rsSYUSRGetPermissionByUserId[0].PermissionCategories;
                            response.PermissionFunctions = rsSYUSRGetPermissionByUserId[0].PermissionFunctions;
                            return response;
                        }
                        else
                        {
                            BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(_context.HttpContext.Request);

                            SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
                            {
                                UserId = user[0].Id,
                                FullName = user[0].FullName,
                                Action = "Login",
                                IPAddress = baseRequest.ipAddress,
                                MACAddress = baseRequest.macAddress,
                                Description = baseRequest.logAction,
                                CreatedDate = DateTime.Now,
                                Status = 0,
                                Exception = null
                            };
                            await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
                            return new ResultApi { Success = ResultCode.INCORRECT, Message = "Người dùng chưa được phân quyền", };

                        }

                    }
                    else
                    {
                        BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(_context.HttpContext.Request);

                        SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
                        {
                            UserId = user[0].Id,
                            FullName = user[0].FullName,
                            Action = "Login",
                            IPAddress = baseRequest.ipAddress,
                            MACAddress = baseRequest.macAddress,
                            Description = baseRequest.logAction,
                            CreatedDate = DateTime.Now,
                            Status = 0,
                            Exception = null
                        };
                        await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
                        //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception(), loginIN.UserName);

                        return new ResultApi
                        {
                            Success = ResultCode.ORROR,
                            Result = 0,
                            Message = "Tên tài khoản hoặc mật khẩu sai"
                        };
                    }
                }
                else
                {
                    return new ResultApi()
                    {
                        Success = ResultCode.ORROR,
                        Result = 0,
                        Message = "Tên tài khoản hoặc mật khẩu sai"
                    };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(_context.HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = "ERROR: " + ex.Message, };
            }
        }

        public async Task<object> RefreshToken(RefreshTokens.RefreshTokenRequest model)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(model.AccessToken) as JwtSecurityToken;
                //var tokenExpiryDate = token.ValidTo;

                //if (tokenExpiryDate > DateTime.Now)
                //{
                //    return new ResultApi { Success = ResultCode.ORROR, Message = "Không thể refresh với token vẫn còn hạn" };
                //}

                // Check the token we got if its saved in the db
                var refreshToken = await new RefreshTokens(_appSetting).GetByToken(model.RefreshToken);

                if (refreshToken == null)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Không tồn tại refreshtoken" };
                }

                // Check the date of the saved token if it has expired
                if (DateTime.Now > refreshToken.Expires)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Refreshtoken đã hết hạn" };
                }

                // check if the refresh token has been used
                if (refreshToken.IsUse)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Refreshtoken này đã được sử dụng" };
                }

                // Check if the token is revoked
                if (refreshToken.IsRevoked)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Refreshtoken này đã bị thu hồi được sử dụng" };
                }

                if (refreshToken.JwtId != token.Claims.FirstOrDefault(x => x.Type == "jti").Value)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Refreshtoken không tương ứng với AccessToken" };
                }

                var user = await new SYUSRLogin(_appSetting).GetInfoByRefreshToken(model.RefreshToken);

                // return null if no user found with token
                if (user == null)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Không tồn tại User" };
                }
                refreshToken.Revoked = DateTime.UtcNow;
                refreshToken.IsUse = true;
                await new RefreshTokens(_appSetting).Update(refreshToken);
                //generate new jwt
                var response = await generateJwtToken(user);


                List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId =
                            await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(Int32.Parse(user.Id.ToString()));
                if (rsSYUSRGetPermissionByUserId != null && rsSYUSRGetPermissionByUserId.Count > 0)
                {
                    response.Permissions = rsSYUSRGetPermissionByUserId[0].Permissions;
                    response.PermissionCategories = rsSYUSRGetPermissionByUserId[0].PermissionCategories;
                    response.PermissionFunctions = rsSYUSRGetPermissionByUserId[0].PermissionFunctions;
                    return response;
                }
                return new ResultApi { Success = ResultCode.INCORRECT, Message = "Người dùng chưa được phân quyền", };
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        public async Task<object> RevokeToken(RevokeTokenRequest model)
        {
            var user = await new SYUSRLogin(_appSetting).GetInfoByRefreshToken(model.RefreshToken);

            // return false if no user found with token
            if (user == null)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = "Không tồn tại User" };
            };

            var refreshToken = await new RefreshTokens(_appSetting).GetByToken(model.RefreshToken);

            // return false if token is not active
            if (refreshToken.IsUse)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = "Token này đã được thu hồi" };
            };

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.IsUse = true;
            await new RefreshTokens(_appSetting).Update(refreshToken);

            return new ResultApi { Success = ResultCode.ORROR, Message = "Thu hồi token thành công" };
        }
        private async Task<LoginResponse> generateJwtToken(SYUSRLogin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config["Jwt:Issuer"],
                Issuer = _config["Jwt:Issuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("FullName", user.FullName),
                    new Claim("Type", user.Type.ToString()),
                    new Claim("UnitId", user.UnitId.ToString()),
                    new Claim("Email", user.Email??""),
                    new Claim(ClaimTypes.Role, user.Type.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var newRefreshToken = generateRefreshToken(token.Id, user.Id);
            // insert  vào db
            await new RefreshTokens(_appSetting).Indert(newRefreshToken);

            return new LoginResponse(user, jwtToken, newRefreshToken.Token);

        }

        private RefreshTokens.RefreshToken generateRefreshToken(string tokenId, long userId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokens.RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.Now.AddDays(20),
                    Created = DateTime.Now,
                    JwtId = tokenId,
                    UserId = userId,
                    IsUse = false
                };
            }
        }


    }
}
