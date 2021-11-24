using Bugsnag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using PAKNAPI.Models.SyncData;
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


        /// <summary>
        /// create room
        /// </summary>
        /// <returns></returns>
        [Route("bot-create-room")]
        [HttpPost]
        public async Task<ActionResult<object>> CreateRoom(string UserName) {
            try
            {
                //return new ResultApi { Success = ResultCode.OK, Result = await new FeedBackSync(_appSetting).SyncFeedBack()};
                // create user
                var guid = UserName;
                string roomName = "Room_" + guid;
                BOTAnonymousUser res = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(guid);
                if (res!= null && res.Id > 0)
                {
                    var id = res.Id;

                    if (id > 0)
                    {
                        // create room
                        var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
                        if (room.Id>0) {
                           
                            SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                            //var botRoomUserLink = new BOTRoomUserLink();
                            //botRoomUserLink.AnonymousId = id;
                            //botRoomUserLink.RoomId = room.Id;
                            //botRoomUserLink.UserId = dataMain.Id;
                            //await new BOTRoomUserLink(_appSetting).BOTRoomUserLinkInsertDAO(botRoomUserLink);

                            IDictionary<string, string> json = new Dictionary<string, string>
                        {
                            {"AnonymousId",  id.ToString()},
                            {"AnonymousName", guid},
                            {"RoomId", room.Id.ToString()},
                        };

                            return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
                        }
                        else {
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
                    else {
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
