
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PAKNAPI.Chat;
using PAKNAPI.Chat.ResponseModel;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Chatbot;
using PAKNAPI.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    public class ChatHub : Hub<IChatHub>
    {
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        private readonly IAppSetting _appSetting;
        //private readonly IManageBots _bots;

        public ChatHub(
            IAppSetting appSetting
        //IManageBots bots

        )
        {
            //_bots = bots;
            _appSetting = appSetting;
        }

        //public async Task BroadcastMessage(Message msg)
        //{
        //    await Clients.All.OnNewMessage(msg);
        //}

        public async Task NotifyAdmin(Room room)
        {
            //update status for room
            await new BOTRoom(_appSetting).BOTRoomUpdateStatus(room.Id, true);
            await Clients.All.NotifyAdmin(room);
        }

        public async Task ReceiveRoomToGroup(Room room) {
            await Clients.All.ReceiveRoomToGroup(room);
        }

        public async Task JoinToRoom(string roomName)
        {
            var httpContext = Context.GetHttpContext();
            var ConnectionId = Context.ConnectionId;
            var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
            if (room != null)
            {
                var resUserId = await GetUserIdByUserName(httpContext);
                int checkExist = await new BOTRoomUserLink(_appSetting).BOTCheckUserExistInRoom(resUserId.Id, room.Id);
                if (checkExist <= 0)
                {
                    var botRoomUserLink = new BOTRoomUserLink();
                    botRoomUserLink.RoomId = room.Id;
                    botRoomUserLink.UserId = resUserId.Id;
                    await new BOTRoomUserLink(_appSetting).BOTRoomUserLinkInsertDAO(botRoomUserLink);
                }

                await Groups.AddToGroupAsync(ConnectionId, room.Name);
            }
        }

        public async Task EnableBot(string roomName, bool isEnableBot)
        {
            try
            {
                var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
                if (room != null)
                {
                    BotStatus status = isEnableBot == true ? BotStatus.Enable : BotStatus.Disable;
                    if (isEnableBot == true) {

                    }
                    await new BOTRoom(_appSetting).BOTRoomEnableBot(roomName, (int)status);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task AdminSendToRoom(string roomName, string message)
        {
            var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
            await new BOTRoom(_appSetting).BOTRoomUpdateStatus(room.Id, false);
            if (room.Type == (int) BotStatus.Enable)
            {
                await new BOTRoom(_appSetting).BOTRoomEnableBot(roomName, (int)BotStatus.Disable);
            }
            await HandleMessageToRoom(room.Name,room.Id, message);
        }

        private async Task HandleMessageToRoom(string roomName,int roomId ,string message) {
            if (!string.IsNullOrEmpty(message))
            {
                var httpContext = Context.GetHttpContext();

                var senderUserName = GetUserName(httpContext);
                var resSenderUserId = await GetUserIdByUserName(httpContext);
                
                DateTime dateSent = DateTime.Now;
                Message messageModel = new Message()
                {
                    Content = message,
                    From = senderUserName,
                    FromId = resSenderUserId.Id.ToString(),
                    FromFullName = resSenderUserId.Name,
                    FromAvatarPath = resSenderUserId.AvatarUrl,
                    To = roomName,
                    Timestamp = ((DateTimeOffset)dateSent).ToUnixTimeSeconds().ToString(), 
                    Type = MessageTypes.Conversation
                };
                await new BOTMessage(_appSetting).BOTMessageInsertDAO(message, resSenderUserId.Id, roomId, resSenderUserId.Name, resSenderUserId.AvatarUrl, dateSent);
                await Clients.Group(roomName).ReceiveMessageToGroup(messageModel);
                var room = await new BOTRoom(_appSetting).BOTRoomGetById(roomId);
                await Clients.All.OnNewMessage(room);
            }
        }

        public async Task AnonymousChatWithBot(string message, string idSuggestLibrary,string hiddenAnswer = "")
        {
            var httpContext = Context.GetHttpContext();
            var senderUserName = GetUserName(httpContext);

            var id = Context.ConnectionId;
            string roomName = "Room_" + senderUserName;
            BOTAnonymousUser senderUser = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(senderUserName);
            var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
            await HandleMessageToRoom(room.Name,room.Id, message);
            if (senderUser != null && room != null && room.Type == (int)BotStatus.Enable)
            {
                DateTime foo = DateTime.Now;
                //var messageId = await new BOTMessage(_appSetting).BOTMessageInsertDAO(message, senderUser.Id, room.Id, foo);
                //ResultBot res = _bots.Response(senderUserName, string.IsNullOrEmpty(hiddenAnswer) ? message : hiddenAnswer);
                List<ResultBotNew> results = new List<ResultBotNew>();
                if (String.IsNullOrEmpty(idSuggestLibrary))
                {
                    results = ResultBot(string.IsNullOrEmpty(hiddenAnswer) ? message : hiddenAnswer);
                }
                else
                {
                    results = new BotGetLibrary(_appSetting).BotGetLibraryById(idSuggestLibrary);
                }                
                DateTime foooo = DateTime.Now;
                double totall = (foooo - foo).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("ChatWithBot 0 " + totall);
                Message messageModel = new Message()
                {
                    HiddenAnswer = string.IsNullOrEmpty(hiddenAnswer) ? message : hiddenAnswer,
                    From = "Bot",
                    FromFullName = "Bot",
                    Results = results,
                    FromId = "Bot",
                    To = roomName,
                    Timestamp = ((DateTimeOffset)foo).ToUnixTimeSeconds().ToString(),
                    Type = MessageTypes.Conversation
                };
                await Clients.Group(roomName).ReceiveMessageToGroup(messageModel);
                await Clients.All.OnNewMessage(room);
                await new BOTMessage(_appSetting).BOTMessageInsertDAO(JsonConvert.SerializeObject(messageModel), 0, room.Id,"Bot","", foo);
                DateTime fooo = DateTime.Now;
                double total = (fooo - foo).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("ChatWithBot 1 " + total);
            }
        }

        private List<ResultBotNew> ResultBot(string input)
        {
            try
            {
                var result = new BotGetLibrary(_appSetting).BotGetLibraryByInput(input);
                return result;
            }
            catch (Exception ex)
            {
                return new List<ResultBotNew>();
            }
        }

        private StringValues GetUserName(HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.Request.Query["userName"]) ? httpContext.Request.Query["userName"] : httpContext.Request.Query["sysUserName"];
        }

        private async Task<UserChatModel> GetUserIdByUserName(HttpContext httpContext)
        {
            if (!string.IsNullOrEmpty(httpContext.Request.Query["userName"]))
            {
                var senderUserName = httpContext.Request.Query["userName"];
                BOTAnonymousUser ress = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(senderUserName);
                return new UserChatModel()
                {
                    Id = ress.Id,
                    Name = httpContext.Request.Query["fullName"].ToString(),
                    AvatarUrl = ""
                };
            }
            else if (!string.IsNullOrEmpty(httpContext.Request.Query["sysUserName"]))
            {
               
                int sysUserId = int.Parse(httpContext.Request.Query["sysUserName"]);
                var ress = await new SYUser(_appSetting).SYUserGetByID(sysUserId);
                return new UserChatModel()
                {
                    Id = sysUserId,
                    Name = ress.FullName,
                    AvatarUrl = ress.Avatar
                };
                
            }
            return new UserChatModel()
            {
                Id = -1,
                AvatarUrl = ""
            };
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var id = Context.ConnectionId;

                var httpContext = Context.GetHttpContext();
                var connectId = httpContext.Request.Query["id"];
                var userName = GetUserName(httpContext);
                //var user = _userRepository.GetByKey(nameof(User.userName), userName.ToString());
                if (!_ConnectionsMap.ContainsKey(userName.ToString().ToLower() + "x_x" + id.ToLower()))
                {
                    _ConnectionsMap.Add(userName + "x_x" + id, id);
                }

                //if (user == null)
                //{
                //    User u = new User();
                //    u.userName = userName;
                //    await _userRepository.Insert<User>(u);
                //}
                //else
                //{

                string roomName = "Room_" + userName;
                BOTRoom room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
                if (room!=null)
                {
                    await (Groups.AddToGroupAsync(id, roomName));
                }
               

                var data = new { userName };
                var respone = new { data, success = ResultCode.OK };
                //wait Clients.Caller.SendAsync(EventType.GetProfileInfo, respone);
            }
            catch (Exception ex)
            {

            }



        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            var connectId = httpContext.Request.Query["id"];
            var userName = GetUserName(httpContext);
            if (_ConnectionsMap.ContainsKey(userName + "x_x" + id))
            {
                _ConnectionsMap.Remove(userName + "x_x" + id);
            }
            //_bots.RemoveBot(userName);

            string roomName = "Room_" + userName;
            BOTRoom room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
            if (room != null)
            {
                await(Groups.RemoveFromGroupAsync(id, roomName));
            }



            //var user = _userRepository.GetByKey(nameof(User.userName), userName.ToString());
            //try
            //{
            //    List<Room> rooms = _roomRepository.GetRoomsByUserId(user.Id);
            //    List<Task> removeGroupTasks = new List<Task>();
            //    foreach (var item in rooms)
            //    {
            //        removeGroupTasks.Add(Groups.RemoveFromGroupAsync(connectId, item.Name));
            //    }
            //    Task.WhenAll(removeGroupTasks);
            //}
            //catch (Exception ex)
            //{

            //}
            base.OnDisconnectedAsync(exception);
            var data = new { userName };
            var respone = new { data, success = ResultCode.OK };
            //return 
        }
    }
}