using ChatCommon.Models;

namespace NetMQMessage.Abstract
{
    public interface IMessageSourceClient
    {
        void Send(Message message);
        Message Receive();
    }
}
