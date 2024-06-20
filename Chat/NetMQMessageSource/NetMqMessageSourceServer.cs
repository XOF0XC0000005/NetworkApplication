using ChatCommon.Abstractions;
using ChatCommon.Models;
using NetMQ;
using NetMQ.Sockets;
using System.Text;

namespace NetMQMessage
{
    public class NetMqMessageSourceServer : IMessageSourceServer
    {
        public readonly ResponseSocket responseSocket;
        public event EventHandler SendAnswer;

        public NetMqMessageSourceServer(ResponseSocket response)
        {
            responseSocket = response;
        }

        public void SendAnswerToUser()
        {
            SendAnswer?.Invoke(this, EventArgs.Empty);
        }

        public Message Receive()
        {
            string str = responseSocket.ReceiveFrameString();
            return Message.DeserializeMessageFromJson(str) ?? new Message();
        }

        public void Send(Message message)
        {
            responseSocket.SendFrame(Encoding.UTF8.GetBytes(message.SerializeMessageToJson()));
        }
    }
}

