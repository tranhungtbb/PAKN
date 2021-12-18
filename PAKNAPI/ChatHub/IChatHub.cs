using PAKNAPI.Chat.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Chat
{
    public interface IChatHub
    {
        Task NotifyAdmin(string msg);
        Task BroadcastMessage(Message msg);
        Task ReceiveMessageToGroup(Message msg);
        Task ReceiveRoomToGroup(Room room);
        Task ReceiveUserToGroup(User user);
        Task onError(string str);
        Task SendAsync<T>(string str, T data);
    }
}
