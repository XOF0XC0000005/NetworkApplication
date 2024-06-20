using ChatCommon.Models;
using NetMQ;
using NetMQ.Sockets;
using NetMQMessage.Abstract;
using System.Text;

namespace NetMQMessage
{
    public class NetMQMessageSourceClient : IMessageSourceClient
    {
        public readonly RequestSocket requestSocket;

        public NetMQMessageSourceClient(RequestSocket request)
        {
            requestSocket = request;
        }

        public Message Receive()
        {
            var str = requestSocket.ReceiveFrameString();
            return Message.DeserializeMessageFromJson(str) ?? new Message();
        }

        public void Send(Message message)
        {
            requestSocket.SendFrame(Encoding.UTF8.GetBytes(message.SerializeMessageToJson()));
        }
    }
}
