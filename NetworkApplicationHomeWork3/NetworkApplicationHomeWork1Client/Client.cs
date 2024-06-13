using NetworkApplicationHomeWork1Server;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace NetworkApplicationHomeWork1Client
{
    public class Client
    {
        public Message? message  = new Message() {DateTime = DateTime.Now, NickTo = "Server"};
        public void SentMessage(string fromWho, string ip)
        {
            const string RegisterCommand = "1";
            const string DeleteCommand = "2";
            const string MessageCommand = "3";
            const string TakeUnreadMessages = "4";
            const string ExitCommand = "5";

            using (UdpClient udpClient = new UdpClient())
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
                
                while (true)
                {
                    string? messageText;

                    do
                    {
                        Console.WriteLine("Выберите вариант действия");
                        Console.WriteLine($"{RegisterCommand}.Регистрация");
                        Console.WriteLine($"{DeleteCommand}.Удаление");
                        Console.WriteLine($"{MessageCommand}.Сообщение");
                        Console.WriteLine($"{TakeUnreadMessages}.Получить непрочитанные сообщения");
                        Console.WriteLine($"{ExitCommand}.Выход");

                        Console.Write("Номер команды: ");
                        messageText = Console.ReadLine();
                    }
                    while (string.IsNullOrEmpty(messageText));

                    message.Text = messageText;
                    message.NickFrom = fromWho;

                    switch (messageText)
                    {
                        case RegisterCommand:
                            message.Command = Commands.Register;
                            break;
                        case DeleteCommand:
                            message.Command = Commands.Delete;
                            break;
                        case MessageCommand:
                            Console.Write("Введите сообщение:");
                            var sentMessage = Console.ReadLine();

                            message.Command = Commands.Message;
                            message.Text = sentMessage;
                            break;
                        case TakeUnreadMessages:
                            message.Command = Commands.UnreadMessages;
                            break;
                        case ExitCommand:
                            Process.GetCurrentProcess().Kill();
                            break;
                    }

                    string json = message.SerializeMessageToJson();
                    byte[] data = Encoding.UTF8.GetBytes(json);
                    udpClient.SendAsync(data, data.Length, endPoint);

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
