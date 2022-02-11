using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NSwag.Annotations;
using PAKNAPI.Chat;
using PAKNAPI.Chat.ResponseModel;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Chatbot;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using PAKNAPI.Models.SyncData;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace PAKNAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [OpenApiTag("Bot", Description = "Bot")]
    public class BotController : BaseApiController
    {

        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        //private readonly IManageBots _bots;

        public BotController(IAppSetting appSetting, IClient bugsnag)
        {
            this._appSetting = appSetting;
            this._bugsnag = bugsnag;
        }

        /// <summary>
        /// danh sách room chat - Authorize
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="CreatedDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("rooms")]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        public async Task<ActionResult<object>> BOTRoomGetAllOnPageBase(string Title,DateTime? CreatedDate, int? PageSize = 20, int? PageIndex = 1)
        {
            try
            {
                List<BOTRoomGetAllOnPage> rsNENewsGetAllOnPage = await new BOTRoomGetAllOnPage(_appSetting).SYUserGetByRoleIdAllOnPageDAO(Title, CreatedDate,PageSize, PageIndex);
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
        /// danh sách room có user muốn gặp quản trị - Authorize
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("rooms-notification")]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        public async Task<ActionResult<object>> BOTRoomForNotificationBase()
        {
            try
            {
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ListRoomIsShow", await new BOTRoomGetAllByStatus(_appSetting).BOTRoomGetAllByStatusDAO()},
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
        /// chi tiết room
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("room-get-by-id")]
        public async Task<ActionResult<object>> BOTRoomGetByIdBase(int Id)
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new BOTRoom(_appSetting).BOTRoomGetById(Id) };
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

        public async Task<ActionResult<object>> CreateRoom([FromBody] CreateRoomBot roomBot)
        {
            try
            {

                // create user
                var guid = roomBot.UserName;
                string time = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds().ToString();
                string roomName = "Room_" + guid;
                BOTAnonymousUser res = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(guid);
                if (res != null && res.Id > 0)
                {
                    var id = res.Id;

                    if (id > 0)
                    {
                        // create room
                        var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
                        if (room != null && room.Id > 0)
                        {
                            IDictionary<string, object> json = new Dictionary<string, object>
                            {
                                {"AnonymousId",  id},
                                {"AnonymousName", guid},
                                {"RoomId", room.Id},
                                {"RoomName", roomName},
                                {"RoomTitle" , room.Title},
                                {"Type", room.Type},
                                {"IsCreateRoom", false},
                            };

                            return new ResultApi { Success = ResultCode.OK, Result = json };
                        }
                        else
                        {
                            var roomId = Int32.Parse((await new BOTRoom(_appSetting).BOTRoomInsertDAO(new BOTRoom(roomName, time, id, (int)BotStatus.Enable))).ToString());
                            IDictionary<string, object> json = new Dictionary<string, object>
                            {
                                {"AnonymousId",  id},
                                {"AnonymousName", guid},
                                {"RoomId", roomId},
                                {"RoomName", roomName},
                                {"RoomTitle" , time},
                                {"Type", (int)BotStatus.Enable},
                                {"IsCreateRoom", true},
                            };
                            return new ResultApi { Success = ResultCode.OK, Result = json };
                        }

                    }
                    else
                    {
                        return new ResultApi { Success = ResultCode.OK, Message = "Đã có lỗi xảy ra" };
                    }
                }
                else
                {
                    var id = Int32.Parse((await new BOTAnonymousUser(_appSetting).BOTAnonymousUserInsertDAO(guid)).ToString());
                    if (id > 0)
                    {
                        // create room

                        var roomId = Int32.Parse((await new BOTRoom(_appSetting).BOTRoomInsertDAO(new BOTRoom(roomName,time, id, (int)BotStatus.Enable))).ToString());
                        
                        IDictionary<string, object> json = new Dictionary<string, object>
                        {
                            {"AnonymousId",  id},
                            {"AnonymousName", guid},
                            {"RoomId", roomId},
                            {"RoomName", roomName},
                            {"RoomTitle" , time},
                            {"Type", (int)BotStatus.Enable},
                            {"IsCreateRoom", true},
                        };
                        //_hubContext.Clients.All.BroadcastMessage(messageModel);
                        return new ResultApi { Success = ResultCode.OK, Result = json };
                    }
                    else
                    {
                        return new ResultApi { Success = ResultCode.OK, Message = "Đã có lỗi xảy ra" };
                    }
                }

            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.OK, Message = ex.Message };
            }
        }

        

        [HttpPost]
        [Route("bot-update-room")]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        public async Task<object> ChatbotGetByRoomIdDAO(long roomId)
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new BOTRoom(_appSetting).BOTRoomUpdateStatus(roomId,false) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách tin nhắn của room
        /// </summary>
        /// <param name="RoomId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-message")]
        public async Task<object> ChatbotGetByRoomIdDAO(int RoomId, int PageIndex, int PageSize)
        {
            try
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK, Result = await new ChatbotGetByRoomId(_appSetting).ChatbotGetByRoomIdDAO(RoomId, PageIndex, PageSize) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("get-library")]
        public async Task<object> ChatbotGetLibrary()
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new BotGetLibrary(_appSetting).BotGetAllLibrary() };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("get-library-by")]
        public async Task<object> ChatbotGetLibraryBy(string input)
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = new BotGetLibrary(_appSetting).BotGetLibraryByInput(input) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("reload-library")]

        public async Task<object> ReloadChatbotGetLibrary()
        {
            try
            {
                string result = await new BotGetLibrary(_appSetting).BotGetAllLibrary();
             
                return new ResultApi { Success = ResultCode.OK, Result = result };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
    }
}
