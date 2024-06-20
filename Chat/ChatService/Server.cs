using ChatCommon.Models;
using NetMQ.Sockets;
using NetMQMessage;
using System.Diagnostics;
using System.Net;

namespace ChatServer
{
    public class Server
    {
        private NetMqMessageSourceServer _messageSource;

        public List<Message> messages = new List<Message>()
        {
            {new Message() {Text = "123123123", DateSend = DateTime.Now, NickNameTo = "Lev", NickNameFrom = "Petr", IsSent = false}},
            {new Message() {Text = "534534", DateSend = DateTime.Now, NickNameTo = "Petr", NickNameFrom = "Server", IsSent = false}},
            {new Message() {Text = "1231fgsdf23123", DateSend = DateTime.Now, NickNameTo = "Lev", NickNameFrom = "Server", IsSent = true}},
            {new Message() {Text = "1231255743123", DateSend = DateTime.Now, NickNameTo = "Petr", NickNameFrom = "Server", IsSent = false}},
            {new Message() {Text = "g", DateSend = DateTime.Now, NickNameTo = "Lev", NickNameFrom = "Server", IsSent = false}},
            {new Message() {Text = "324", DateSend = DateTime.Now, NickNameTo = "Lev", NickNameFrom = "Petr", IsSent = true}},
            {new Message() {Text = "vbcn", DateSend = DateTime.Now, NickNameTo = "Lev", NickNameFrom = "Petr", IsSent = false}},
        };

        public string Name { get => "Server-1"; }
        public Dictionary<string, IPEndPoint> Users { get; set; }

        public Server()
        {
            _messageSource = new NetMqMessageSourceServer(new ResponseSocket("@tcp://*:5555"));
            _messageSource.SendAnswer += (s, e) => SendAnswer();
        }
        public void SendAnswer()
        {
            _messageSource.Send(new Message() { Text = "Сообщение получено", NickNameFrom = "Server-1" });
        }

        public void Start()
        {
            Console.WriteLine("Нажмите ESC для выхода");
            Console.WriteLine("Сервер ждет сообщение от клиента");

            using (_messageSource.responseSocket)
            {
                while (true)
                {
                    Task.Run(() =>
                    {
                        if (Console.ReadKey().Key == ConsoleKey.Escape) Process.GetCurrentProcess().Kill();
                    });

                    var msg = _messageSource.Receive();

                    msg?.Print();

                    SendAnswer();
                }
            }
        }
    }
}
