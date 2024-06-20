using ChatCommon.Models;


namespace NetMQMessage
{
    internal interface IMessageSource
    {
        void Send(Message message);
        Message Receive();
    }
}
