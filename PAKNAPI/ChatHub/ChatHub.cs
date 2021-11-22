
using KarmaloopAIMLBotParser;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PAKNAPI.Chat;
using PAKNAPI.Chat.ResponseModel;
using PAKNAPI.Common;
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

        public ChatHub(
     //IUserRepository userRepository,
     //IRoomRepository roomRepository,
     //IMessageRepository messageRepository,
     //IRoomLinkRepository roomLinkRepository
     )
        {
            bots = new ManageBots();
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
            DateTime foo = DateTime.Now;
            ResultBot res = bots.Response(senderUserName, message);
           
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


            await Clients.Caller.ReceiveMessageToGroup(messageViewModel);
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