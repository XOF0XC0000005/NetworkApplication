using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NetworkApplicationHomeWork1Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server();
        }

        public static void Server()
        {
            using (UdpClient udpClient = new UdpClient(12345))
            {
                IPEndPoint ipEndpoint = new IPEndPoint(IPAddress.Any, 0);

                Console.WriteLine("Сервер ждет сообщение от клиента");

                while (true)
                {
                    byte[] buffer = udpClient.Receive(ref ipEndpoint);

                    if (buffer == null) break;

                    var messageText = Encoding.UTF8.GetString(buffer);
                    Message? message = Message.DeserializeMessageFromJson(messageText);
                    message?.Print();

                    byte[] answer = Encoding.UTF8.GetBytes("Сообщение получено");
                    udpClient.Send(answer, answer.Length, ipEndpoint);
                }
            }
        }
    }
}
