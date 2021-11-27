using Bugsnag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PAKNAPI.Chat;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Chatbot;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using PAKNAPI.Models.SyncData;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : BaseApiController
    {

        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public BotController(IAppSetting appSetting, IClient bugsnag) {
            this._appSetting = appSetting;
            this._bugsnag = bugsnag;
          
        }

        
            [HttpGet]
        [Route("rooms")]
        public async Task<ActionResult<object>> BOTRoomGetAllOnPageBase(int? PageSize, int? PageIndex)
        {
            try
            {
                List<BOTRoomGetAllOnPage> rsNENewsGetAllOnPage = await new BOTRoomGetAllOnPage(_appSetting).SYUserGetByRoleIdAllOnPageDAO(PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"Data", rsNENewsGetAllOnPage},
                        {"TotalCount", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? rsNENewsGetAllOnPage[0].RowNumber : 0},
                        {"PageIndex", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? PageSize : 0},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// create room
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("bot-create-room")]
       
        public async Task<ActionResult<object>> CreateRoom([FromBody] CreateRoomBot roomBot) {
            try
            {

                // create user
                var guid = roomBot.UserName;
                string roomName = "Room_" + guid;
                BOTAnonymousUser res = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(guid);
                if (res != null && res.Id > 0)
                {
                    var id = res.Id;

                    if (id > 0)
                    {
                        // create room
                        var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
                        if (room.Id > 0)
                        {

                            SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();


                            IDictionary<string, string> json = new Dictionary<string, string>
                        {
                            {"AnonymousId",  id.ToString()},
                            {"AnonymousName", guid},
                            {"RoomId", room.Id.ToString()},
                             {"RoomName", roomName
                                }
                        };

                            return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
                        }
                        else
                        {
                            var roomId = Int32.Parse((await new BOTRoom(_appSetting).BOTRoomInsertDAO(new BOTRoom(roomName, id, 1))).ToString());
                            SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();

                            IDictionary<string, string> json = new Dictionary<string, string>
                        {
                            {"AnonymousId",  id.ToString()},
                            {"AnonymousName", guid},
                            {"RoomId", roomId.ToString()},
                             {"RoomName", roomName}
                        };

                            return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
                        }

                    }
                    else
                    {
                        return new Models.Results.ResultApi { Success = ResultCode.OK, Message = "Đã có lỗi xảy ra" };
                    }
                }
                else
                {
                    var id = Int32.Parse((await new BOTAnonymousUser(_appSetting).BOTAnonymousUserInsertDAO(guid)).ToString());
                    if (id > 0)
                    {
                        // create room

                        var roomId = Int32.Parse((await new BOTRoom(_appSetting).BOTRoomInsertDAO(new BOTRoom(roomName, id, 1))).ToString());
                        SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                        //var botRoomUserLink = new BOTRoomUserLink();
                        //botRoomUserLink.AnonymousId = id;
                        //botRoomUserLink.RoomId = roomId;
                        //botRoomUserLink.UserId = dataMain.Id;
                        //await new BOTRoomUserLink(_appSetting).BOTRoomUserLinkInsertDAO(botRoomUserLink);

                        IDictionary<string, string> json = new Dictionary<string, string>
                        {
                            {"AnonymousId",  id.ToString()},
                            {"AnonymousName", guid},
                            {"RoomId", roomId.ToString()},
                        };

                        return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
                    }
                    else
                    {
                        return new Models.Results.ResultApi { Success = ResultCode.OK, Message = "Đã có lỗi xảy ra" };
                    }
                }

            }
            catch (Exception ex) {
                return new Models.Results.ResultApi { Success = ResultCode.OK , Message = ex.Message};
            }
        }
    }
}
