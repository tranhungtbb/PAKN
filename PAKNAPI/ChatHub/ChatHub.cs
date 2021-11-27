
using KarmaloopAIMLBotParser;
using Microsoft.AspNetCore.SignalR;
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
        ManageBots bots;
        public readonly static List<User> _Connections = new List<User>();
        public readonly static List<Room> _Rooms = new List<Room>();
        private readonly static Dictionary<string, string> _ConnectionsMap = new Dictionary<string, string>();
        private readonly IAppSetting _appSetting;
        public ChatHub(
            IAppSetting appSetting
        //IUserRepository userRepository,
        //IRoomRepository roomRepository,
        //IMessageRepository messageRepository,
        //IRoomLinkRepository roomLinkRepository
        )
        {
            bots = new ManageBots();
            _appSetting = appSetting;
            //_userRepository = userRepository;
            //_roomLinkRepository = roomLinkRepository;
            //_roomRepository = roomRepository;
            //_messageRepository = messageRepository;

        }

        public async Task BroadcastMessage(Message msg)
        {

            await Clients.All.BroadcastMessage(msg);
        }

        public async Task ChatWithBot(string message)
        {
            var httpContext = Context.GetHttpContext();
            var connectId = httpContext.Request.Query["id"];
            var senderUserName = httpContext.Request.Query["userName"];

            var id = Context.ConnectionId;
            string roomName = "Room_" + senderUserName;
            BOTAnonymousUser ress = await new BOTAnonymousUser(_appSetting).BOTAnonymousUserGetByUserName(senderUserName);
            var room = await new BOTRoom(_appSetting).BOTRoomGetByName(roomName);
            if (ress != null && room!= null) {
                



                DateTime foo = DateTime.Now;
                var messageId = await new BOTMessage(_appSetting).BOTMessageInsertDAO(message, ress.Id, room.Id, foo);
                ResultBot res = bots.Response(senderUserName, message);
                DateTime foooo = DateTime.Now;
                double totall = (foooo - foo).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("ChatWithBot 0 " + totall);
                Message messageViewModel = new Message()
                {
                    Content = Regex.Replace(res.Answer, @"(?i)<(?!img|a|/a|/img).*?>", string.Empty),
                    From = "Bot",
                    SubTags = (res.SubTags),
                    FromId = "Bot",
                    To = senderUserName,
                    ToId = connectId,
                    Timestamp = ((DateTimeOffset)foo).ToUnixTimeSeconds().ToString()
                };
                //var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL));

                //await Clients.Caller.ReceiveMessageToGroup(messageViewModel);
                await Clients.All.ReceiveMessageToGroup(messageViewModel);
                var messageIdd = await new BOTMessage(_appSetting).BOTMessageInsertDAO(JsonConvert.SerializeObject(messageViewModel), 0, room.Id, foo);
                DateTime fooo = DateTime.Now;
                double total = (fooo - foo).TotalMilliseconds;
                System.Diagnostics.Debug.WriteLine("ChatWithBot 1 " + total);
            }
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var id = Context.ConnectionId;

                var httpContext = Context.GetHttpContext();
                var connectId = httpContext.Request.Query["id"];
                var userName = httpContext.Request.Query["userName"];
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
            var userName = httpContext.Request.Query["userName"];
            if (_ConnectionsMap.ContainsKey(userName + "x_x" + id))
            {
                _ConnectionsMap.Remove(userName + "x_x" + id);
            }
            bots.RemoveBot(userName);
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