using NetMQ;
using NetMQ.Sockets;

namespace NetMQMessage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var responseSocket = new ResponseSocket("@tcp://*:5555"))
            using (var requestSocket = new RequestSocket(">tcp://localhost:5555"))
            {
                Console.WriteLine("requestSocket : Sending 'Hello'");
                requestSocket.SendFrame("Hello my world");
                var message = responseSocket.ReceiveFrameString();
                Console.WriteLine("responseSocket : Server Received '{0}'", message);
                Console.WriteLine("responseSocket Sending 'World'");
                responseSocket.SendFrame("World");
                message = requestSocket.ReceiveFrameString();
                Console.WriteLine("requestSocket : Received '{0}'", message);
                Console.ReadLine();
            }
        }
    }
}
