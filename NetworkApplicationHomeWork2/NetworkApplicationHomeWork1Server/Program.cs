using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;

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

                Console.WriteLine("Нажмите ESC для выхода");
                Console.WriteLine("Сервер ждет сообщение от клиента");

                while (true)
                {
                    Thread exitThread = new Thread(() =>
                    {
                        if (Console.ReadKey().Key == ConsoleKey.Escape) Process.GetCurrentProcess().Kill();
                    });
                    exitThread.Start();

                    byte[] buffer = udpClient.Receive(ref ipEndpoint);

                    if (buffer == null) break;

                    string messageText = Encoding.UTF8.GetString(buffer);

                    ThreadPool.QueueUserWorkItem((_) =>
                    {
                        Message? message = Message.DeserializeMessageFromJson(messageText);
                        message?.Print();

                        byte[] answer = Encoding.UTF8.GetBytes("Сообщение получено");
                        udpClient.Send(answer, answer.Length, ipEndpoint);
                    });
                }
            }
        }
    }
}
