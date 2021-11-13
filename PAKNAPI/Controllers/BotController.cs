﻿using Bugsnag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
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
        [HttpGet]
        public async Task<ActionResult<object>> CreateRoom() {
            try
            {
                // create user
                var guid = Guid.NewGuid().ToString();
                var id = Int32.Parse((await new BOTAnonymousUser(_appSetting).BOTAnonymousUserInsertDAO(guid)).ToString());

                if (id > 0)
                {
                    // create room

                    var roomId =Int32.Parse((await new BOTRoom(_appSetting).BOTRoomInsertDAO(new BOTRoom("Room_" + guid, 1))).ToString());
                    SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                    var botRoomUserLink = new BOTRoomUserLink();
                    botRoomUserLink.AnonymousId = id;
                    botRoomUserLink.RoomId = roomId;
                    botRoomUserLink.UserId = dataMain.Id;
                    await new BOTRoomUserLink(_appSetting).BOTRoomUserLinkInsertDAO(botRoomUserLink);

                    IDictionary<string, string> json = new Dictionary<string, string>
                    {
                        {"AnonymousId",  id.ToString()},
                        {"AnonymousName", guid},
                        {"RoomId", roomId.ToString()},
                    };

                    return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json};
                }
                else {
                    return new Models.Results.ResultApi { Success = ResultCode.OK,  Message = "Đã có lỗi xảy ra"};
                }
            }
            catch (Exception ex) {
                return new Models.Results.ResultApi { Success = ResultCode.OK , Message = ex.Message};
            }
        }
    }
}
