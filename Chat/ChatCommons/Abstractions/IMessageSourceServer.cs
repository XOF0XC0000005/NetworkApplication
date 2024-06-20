using ChatCommon.Models;

namespace ChatCommon.Abstractions
{
    public interface IMessageSourceServer
    {
        void Send(Message message);
        Message Receive();
    }
}
