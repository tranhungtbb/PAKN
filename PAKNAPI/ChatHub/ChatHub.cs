
using KarmaloopAIMLBotParser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PAKNAPI.Chat;
using PAKNAPI.Chat.ResponseModel;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
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
        private readonly IManageBots _bots;
        public ChatHub(
            IAppSetting appSetting,
            IManageBots bots
        )
        {
            _bots = bots;
            _appSetting = appSetting;
        }

        public async Task BroadcastMessage(Message msg)
        {
            await Clients.All.BroadcastMessage(msg);
        }

        public async Task JoinToRoom(string roomName)
        {
            var httpContext = Context.GetHttpContext();
            var ConnectionId = Context.ConnectionId;
            var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
            if (room != null)
            {
                int userId = await GetUserIdByUserName(httpContext);
                int checkExist = await new BOTRoomUserLink(_appSetting).BOTCheckUserExistInRoom(userId, room.Id);
                if (checkExist <= 0)
                {
                    var botRoomUserLink = new BOTRoomUserLink();
                    botRoomUserLink.RoomId = room.Id;
                    botRoomUserLink.UserId = userId;
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
                    await new BOTRoom(_appSetting).BOTRoomEnableBot(roomName, (int)status);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task SendToRoom(string roomName, string message)
        {
            if (!string.IsNullOrEmpty(message)) {
                var httpContext = Context.GetHttpContext();

                var senderUserName = GetUserName(httpContext);
                var senderUserId = await GetUserIdByUserName(httpContext);
                var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
                DateTime dateSent = DateTime.Now;
                Message messageModel = new Message()
                {
                    Content = message,
                    From = senderUserName,
                    FromId = senderUserId.ToString(),
                    To = roomName,
                    Timestamp = ((DateTimeOffset)dateSent).ToUnixTimeSeconds().ToString(),
                    Type = MessageTypes.Conversation
                };
                var messageId = await new BOTMessage(_appSetting).BOTMessageInsertDAO(message, senderUserId, room.Id, dateSent);
                await Clients.Group(room.Name).ReceiveMessageToGroup(messageModel);
            }
            
        }

        

        public async Task AnonymousChatWithBot(string message)
        {
            var httpContext = Context.GetHttpContext();
            var senderUserName = GetUserName(httpContext);

            var id = Context.ConnectionId;
            string roomName = "Room_" + senderUserName;
            BOTAnonymousUser senderUser = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(senderUserName);
            var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);

            if (senderUser != null && room != null && room.Type == (int)BotStatus.Enable )
            {

                DateTime foo = DateTime.Now;
                var messageId = await new BOTMessage(_appSetting).BOTMessageInsertDAO(message, senderUser.Id, room.Id, foo);
                ResultBot res = _bots.Response(senderUserName, message);
                DateTime foooo = DateTime.Now;
                double totall = (foooo - foo).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("ChatWithBot 0 " + totall);
                Message messageModel = new Message()
                {
                    Content = res.Answer,
                    From = "Bot",
                    SubTags = (res.SubTags),
                    FromId = "Bot",
                    To = roomName,
                    Timestamp = ((DateTimeOffset)foo).ToUnixTimeSeconds().ToString(),
                    Type = MessageTypes.Conversation
                };
                await Clients.Group(roomName).ReceiveMessageToGroup(messageModel);
                var messageIdd = await new BOTMessage(_appSetting).BOTMessageInsertDAO(JsonConvert.SerializeObject(messageModel), 0, room.Id, foo);
                DateTime fooo = DateTime.Now;
                double total = (fooo - foo).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("ChatWithBot 1 " + total);
            }
            else
            {
                //await SendToRoom(roomName, message);
            }
        }

        private StringValues GetUserName(HttpContext httpContext)
        {
            return !string.IsNullOrEmpty(httpContext.Request.Query["userName"]) ? httpContext.Request.Query["userName"] : httpContext.Request.Query["sysUserName"];
        }

        private async Task<int> GetUserIdByUserName(HttpContext httpContext)
        {
            if (!string.IsNullOrEmpty(httpContext.Request.Query["userName"]))
            {
                var senderUserName = httpContext.Request.Query["userName"];
                BOTAnonymousUser ress = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(senderUserName);
                return ress.Id;
            }
            else if (!string.IsNullOrEmpty(httpContext.Request.Query["sysUserName"]))
            {
                int sysUserId = int.Parse(httpContext.Request.Query["sysUserName"]);
                return sysUserId;
            }
            return -1;
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
                //    List<Room> rooms = _roomRepository.GetRoomsByUserId(user.Id);
                //    List<Task> joinGroupTasks = new List<Task>();
                //    foreach (var item in rooms)
                //    {

                //        await (Groups.AddToGroupAsync(id, item.Name));
                //    }
                //    await Task.WhenAll(joinGroupTasks);
                //}
                //await (Groups.AddToGroupAsync(id, "roomAll"));
                var data = new { userName };
                var respone = new { data, success = ResultCode.OK };
                //wait Clients.Caller.SendAsync(EventType.GetProfileInfo, respone);
            }
            catch (Exception ex)
            {
            }



        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var id = Context.ConnectionId;
            var httpContext = Context.GetHttpContext();
            var connectId = httpContext.Request.Query["id"];
            var userName = GetUserName(httpContext);
            if (_ConnectionsMap.ContainsKey(userName + "x_x" + id))
            {
                _ConnectionsMap.Remove(userName + "x_x" + id);
            }
            _bots.RemoveBot(userName);
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

            return base.OnDisconnectedAsync(exception);
        }
    }
}