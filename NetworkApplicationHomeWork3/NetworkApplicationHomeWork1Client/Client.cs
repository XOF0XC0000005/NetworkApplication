using NetworkApplicationHomeWork1Server;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NetworkApplicationHomeWork1Client
{
    public class Client
    {
        public Message? message;
        public void SentMessage(string from, string ip)
        {
            const string Register = "1";
            const string Delete = "2";
            const string Message = "3";
            const string Exit = "4";

            using (UdpClient udpClient = new UdpClient())
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
                
                while (true)
                {
                    string? messageText;

                    do
                    {
                        Console.WriteLine("Выберите вариант действия");
                        Console.WriteLine($"{Register}.Регистрация");
                        Console.WriteLine($"{Delete}.Удаление");
                        Console.WriteLine($"{Message}.Сообщение");
                        Console.WriteLine($"{Exit}.Выход");

                        Console.Write("Номер команды: ");
                        messageText = Console.ReadLine();
                    }
                    while (string.IsNullOrEmpty(messageText));

                    switch(messageText)
                    {
                        case Register:
                            message = new Message() { Text = messageText, DateTime = DateTime.Now, NickFrom = from, NickTo = "Server", Command = Commands.Register };
                            break;
                        case Delete:
                            message = new Message() { Text = messageText, DateTime = DateTime.Now, NickFrom = from, NickTo = "Server", Command = Commands.Delete };
                            break;
                        case Message:
                            Console.Write("Введите сообщение:");
                            var sentMessage = Console.ReadLine();

                            message = new Message() { Text = sentMessage, DateTime = DateTime.Now, NickFrom = from, NickTo = "Server", Command = Commands.Message };
                            break;
                        case Exit:
                            Process.GetCurrentProcess().Kill();
                            break;
                    }
                   
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
