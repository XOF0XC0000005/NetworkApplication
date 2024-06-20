using ChatCommon.Models;
using NetMQ;
using NetMQ.Sockets;
using NetMQMessage;
using System.Diagnostics;
using System.Text;

namespace ChatClient
{
    public class Client
    {
        public Message? message = new Message() { DateSend = DateTime.Now, NickNameTo = "Server" };
        private NetMQMessageSourceClient _messageSource;

        public Client()
        {
            _messageSource = new NetMQMessageSourceClient(new RequestSocket(">tcp://localhost:5555"));
        }

        public void SentMessage(string fromWho, string ip)
        {
            const string RegisterCommand = "1";
            const string DeleteCommand = "2";
            const string MessageCommand = "3";
            const string ExitCommand = "4";

            while (true)
            {
                string? messageText;

                do
                {
                    Console.WriteLine("Выберите вариант действия");
                    Console.WriteLine($"{RegisterCommand}.Регистрация");
                    Console.WriteLine($"{DeleteCommand}.Удаление");
                    Console.WriteLine($"{MessageCommand}.Сообщение");
                    Console.WriteLine($"{ExitCommand}.Выход");

                    Console.Write("Номер команды: ");
                    messageText = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(messageText));

                message.Text = messageText;
                message.NickNameFrom = fromWho;

                switch (messageText)
                {
                    case RegisterCommand:
                        message.Command = Command.Register;
                        break;
                    case DeleteCommand:
                        message.Command = Command.Delete;
                        break;
                    case MessageCommand:
                        Console.Write("Введите сообщение:");
                        var sentMessage = Console.ReadLine();

                        message.Command = Command.Message;
                        message.Text = sentMessage;
                        break;
                    case ExitCommand:
                        Process.GetCurrentProcess().Kill();
                        break;
                }

                _messageSource.Send(message);
                _messageSource.Receive();

                Console.WriteLine($"response: {message.Text}");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}

