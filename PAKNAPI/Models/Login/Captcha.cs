using Dapper;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.Models.Results
{
    public class Captcha
    {
        public static SQLCon _sQLCon;
        const string Letters = "12346789ABCDEFGHJKLMNPRTUVWXYZ";
        const string LettersOTP = "123467890";

        public Captcha(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }

        public string GenerateCaptchaCode()
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 5; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        public string GenerateOTPCode()
        {
            Random rand = new Random();
            int maxRand = LettersOTP.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(LettersOTP[index]);
            }

            return sb.ToString();
        }

        public bool ValidateCaptchaCode(string userInputCaptcha, List<CaptchaObject> context, DateTime createdDate)
        {
            var isValid = false;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Code", userInputCaptcha);
            parameters.Add("@CreatedDate", createdDate);
            var result = _sQLCon.ExecuteListDapper<int>("SY_CaptChaValidator", parameters).FirstOrDefault();
            if (result > 0)
            {
                isValid = true;
            }
            return isValid;
        }

        public async Task<int?> InsertCaptcha(string captcha, string ipAddress, string userAgent, DateTime createdDate)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Code", captcha);
                parameters.Add("@IpAddress", ipAddress);
                parameters.Add("@UserAgent", userAgent);
                parameters.Add("@createdDate", createdDate);
                return await _sQLCon.ExecuteNonQueryDapperAsync("SY_CaptChaInsertData", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<int?> DeleteCaptcha(string captcha, DateTime createdDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Code", captcha);
            parameters.Add("@CreatedDate", createdDate);
            return await _sQLCon.ExecuteNonQueryDapperAsync("SY_CaptChaDelete", parameters);
        }

        public async Task<int?> DeleteCaptchaByUserAgent(string ipAddress, string userAgent)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IpAddress", ipAddress);
            parameters.Add("@UserAgent", userAgent);
            return await _sQLCon.ExecuteNonQueryDapperAsync("SY_CaptChaDeleteByUserAgent", parameters);
        }

        public CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random rand = new Random();

                graph.Clear(GetRandomLightColor());
                //graph.Clear(Color.FromArgb(0,0,0,0));
                DrawCaptchaCode();
                //DrawDisorderLine();
                AdjustRippleEffect();

                MemoryStream ms = new MemoryStream();
                baseMap.Save(ms, ImageFormat.Gif);

                return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };

                int GetFontSize(int imageWidth, int captchCodeCount)
                {
                    var averageSize = imageWidth / captchCodeCount;

                    return Convert.ToInt32(averageSize);
                }

                Color GetRandomDeepColor()
                {
                    return Color.Black;
                }

                Color GetRandomLightColor()
                {
                    int low = 180, high = 255;

                    int nRend = rand.Next(high) % (high - low) + low;
                    int nGreen = rand.Next(high) % (high - low) + low;
                    int nBlue = rand.Next(high) % (high - low) + low;

                    return Color.White;
                }

                void DrawCaptchaCode()
                {
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    int fontSize = GetFontSize(width - 10, captchaCode.Length);
                    Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    for (int i = 0; i < captchaCode.Length; i++)
                    {
                        fontBrush.Color = GetRandomDeepColor();

                        int shiftPx = fontSize / 14;

                        float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                        int maxY = height - fontSize;
                        if (maxY < 0) maxY = 0;
                        float y = rand.Next(0, maxY);

                        graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                    }
                }

                void DrawDisorderLine()
                {
                    Pen linePen = new Pen(new SolidBrush(Color.Black), 3);
                    for (int i = 0; i < rand.Next(3, 5); i++)
                    {
                        linePen.Color = GetRandomDeepColor();

                        Point startPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                        Point endPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                        graph.DrawLine(linePen, startPoint, endPoint);

                        //Point bezierPoint1 = new Point(rand.Next(0, width), rand.Next(0, height));
                        //Point bezierPoint2 = new Point(rand.Next(0, width), rand.Next(0, height));

                        //graph.DrawBezier(linePen, startPoint, bezierPoint1, bezierPoint2, endPoint);
                    }
                }

                void AdjustRippleEffect()
                {
                    short nWave = 6;
                    int nWidth = baseMap.Width;
                    int nHeight = baseMap.Height;

                    Point[,] pt = new Point[nWidth, nHeight];

                    for (int x = 0; x < nWidth; ++x)
                    {
                        for (int y = 0; y < nHeight; ++y)
                        {
                            var xo = nWave * Math.Sin(1);
                            var yo = nWave * Math.Cos(1);

                            var newX = x + xo;
                            var newY = y + yo;

                            if (newX > 0 && newX < nWidth)
                            {
                                pt[x, y].X = (int)newX;
                            }
                            else
                            {
                                pt[x, y].X = 0;
                            }


                            if (newY > 0 && newY < nHeight)
                            {
                                pt[x, y].Y = (int)newY;
                            }
                            else
                            {
                                pt[x, y].Y = 0;
                            }
                        }
                    }

                    Bitmap bSrc = (Bitmap)baseMap.Clone();

                    BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                    int scanline = bitmapData.Stride;

                    IntPtr scan0 = bitmapData.Scan0;
                    IntPtr srcScan0 = bmSrc.Scan0;

                    unsafe
                    {
                        byte* p = (byte*)(void*)scan0;
                        byte* pSrc = (byte*)(void*)srcScan0;

                        int nOffset = bitmapData.Stride - baseMap.Width * 3;

                        for (int y = 0; y < nHeight; ++y)
                        {
                            for (int x = 0; x < nWidth; ++x)
                            {
                                var xOffset = pt[x, y].X;
                                var yOffset = pt[x, y].Y;

                                if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                                {
                                    if (pSrc != null)
                                    {
                                        p[0] = pSrc[yOffset * scanline + xOffset * 3];
                                        p[1] = pSrc[yOffset * scanline + xOffset * 3 + 1];
                                        p[2] = pSrc[yOffset * scanline + xOffset * 3 + 2];
                                    }
                                }

                                p += 3;
                            }
                            p += nOffset;
                        }
                    }
                    baseMap.UnlockBits(bitmapData);
                    bSrc.UnlockBits(bmSrc);
                    bSrc.Dispose();
                }
            }
        }

    }
    public class CaptchaObject
    {
        public string Code { get; set; }
        public DateTime CreateTime { get; set; }
    }
    public class CaptchaResult
    {
        public string CaptchaCode { get; set; }
        public byte[] CaptchaByteData { get; set; }
        public string CaptchBase64Data => Convert.ToBase64String(CaptchaByteData);
        public DateTime Timestamp { get; set; }
    }


    public class SYOTP
    {
        public static SQLCon _sQLCon;

        public SYOTP(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }


        public async Task<int?> InsertOTPDAO(OTP otp)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Code", otp.Code);
            parameters.Add("UserAgent", otp.UserAgent);
            parameters.Add("CreatedDate", otp.CreatedDate);
            parameters.Add("ExpireDate", otp.ExpireDate);
            parameters.Add("IsUse", otp.IsUse);
            return await _sQLCon.ExecuteNonQueryDapperAsync("SY_OTPInsert", parameters);
        }

        public async Task<int?> DeleteOTPDAO(string code)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Code", code);
            return await _sQLCon.ExecuteNonQueryDapperAsync("SY_OTPDelete", parameters);
        }

        public async Task<OTP> GetOTPByCodeDAO(string code, string userAgent)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Code", code);
            parameters.Add("UserAgent", userAgent);
            return (await _sQLCon.ExecuteListDapperAsync<OTP>("SY_OTPGetByCode", parameters)).FirstOrDefault();
        }

        public async Task<int?> UpdateOTPDAO(OTP otp)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", otp.Id);
            parameters.Add("IsUse", otp.IsUse);
            return await _sQLCon.ExecuteNonQueryDapperAsync("SY_OTPRevoke", parameters);
        }
    }

    public class OTP
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsUse { get; set; }

        public OTP(string code, string userAgent)
        {
            this.Code = code;
            this.UserAgent = userAgent;
            this.CreatedDate = DateTime.Now;
            this.ExpireDate = DateTime.Now.AddMinutes(10);
            this.IsUse = false;
        }

        public OTP() { }
    }

    public class OtpForgetPasswordRequest
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Token không được để trống")]
        public string Token { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được để trống")]
        public string Phone { get; set; }
    }

    public class OtpRegisterRequest
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Token không được để trống")]
        public string Token { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được để trống")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Type không được để trống")]
        public int Type { get; set; }

        public string IdCard { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng định dạng")]
        public string Email { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail văn phòng đại diện không đúng định dạng")]
        public string OrgEmail { get; set; }

        public string BusinessRegistration { get; set; }

    }
}
