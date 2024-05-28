using System.Net.Sockets;
using System.Net;
using System.Text;
using NetworkApplicationHomeWork1Server;

namespace NetworkApplicationHomeWork1Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SentMessage(args[0], args[1]);
        }

        public static void SentMessage(string from, string ip)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);

                while (true)
                {
                    string? messageText;

                    do
                    {
                        Console.WriteLine("Введите сообщение.");
                        messageText = Console.ReadLine();
                    }
                    while (string.IsNullOrEmpty(messageText));

                    Message message = new Message() { Text = messageText, DateTime = DateTime.Now, NickFrom = from, NickTo = "Server" };
                    string json = message.SerializeMessageToJson();
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    udpClient.Send(data, data.Length, endPoint);

                    byte[] buffer = udpClient.Receive(ref endPoint);
                    if (buffer == null) break;

                    var messageReceive = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(messageReceive);
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
    }
}
