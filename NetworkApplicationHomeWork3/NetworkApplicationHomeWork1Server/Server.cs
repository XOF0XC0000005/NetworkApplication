using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace NetworkApplicationHomeWork1Server
{
    public enum Commands
    {
        Register,
        Delete,
        Message,
        UnreadMessages
    }

    public enum TypeSend
    {
        ToAll,
        ToOne,
        Default
    }
    public class Server
    {
        private UdpClient _udpClient;
        private IPEndPoint _ipEndpoint;
        private Manager _manager;
        public List<Message> messages = new List<Message>()
        {
            {new Message() {Text = "123123123", DateTime = DateTime.Now, NickTo = "Lev", NickFrom = "Petr", IsSent = false}},
            {new Message() {Text = "534534", DateTime = DateTime.Now, NickTo = "Petr", NickFrom = "Server", IsSent = false}},
            {new Message() {Text = "1231fgsdf23123", DateTime = DateTime.Now, NickTo = "Lev", NickFrom = "Server", IsSent = true}},
            {new Message() {Text = "1231255743123", DateTime = DateTime.Now, NickTo = "Petr", NickFrom = "Server", IsSent = false}},
            {new Message() {Text = "g", DateTime = DateTime.Now, NickTo = "Lev", NickFrom = "Server", IsSent = false}},
            {new Message() {Text = "324", DateTime = DateTime.Now, NickTo = "Lev", NickFrom = "Petr", IsSent = true}},
            {new Message() {Text = "vbcn", DateTime = DateTime.Now, NickTo = "Lev", NickFrom = "Petr", IsSent = false}},
        };
     
        public string Name { get => "Server-1"; }
        public Dictionary<string, IPEndPoint> Users { get; set; }

        public Server()
        {
            _udpClient = new UdpClient(12345);
            _ipEndpoint = new IPEndPoint(IPAddress.Any, 0);
            _manager = new Manager(this);
            _manager.SendAnswer += (s, e) => SendAnswer();
        }
        public async void SendAnswer(Commands command = Commands.Message, string name = "")
        {
            byte[] answer;
            string unreadMessages = String.Empty;

            if (command == Commands.UnreadMessages)
            {
                await Task.Run(() =>
                {
                    foreach (var item in messages)
                    {
                        if (item.NickTo == name && item.IsSent == false)
                        {
                            unreadMessages += $"Сообщение получено от {item.NickFrom}\n Тело: {item.Text}\n";
                        }
                    }
                });
                

                answer = Encoding.UTF8.GetBytes(unreadMessages);
                await _udpClient.SendAsync(answer, answer.Length, _ipEndpoint);
            }

             answer = Encoding.UTF8.GetBytes("Сообщение получено");
            _udpClient.Send(answer, answer.Length, _ipEndpoint);
        }
        public Message Listen()
        {
            byte[] buffer = _udpClient.Receive(ref _ipEndpoint);
            string messageText = Encoding.UTF8.GetString(buffer);
            Message? msg = Message.DeserializeMessageFromJson(messageText);

            return msg;
        }

        public void Send(TypeSend type, Message message)
        {
            byte[] reply = Encoding.UTF8.GetBytes(message.SerializeMessageToJson());

            switch (type)
            {
                case TypeSend.ToAll:
                    foreach (var ip in Users.Values)
                    {
                        _udpClient.Send(reply, reply.Length, _ipEndpoint);
                    }
                    break;
                case TypeSend.ToOne:
                    if (Users.TryGetValue(message.NickTo, out IPEndPoint ipEndpoint))
                        _udpClient.Send(reply, reply.Length, _ipEndpoint);
                    break;
            }
        }

        public void Start()
        {
            using (_udpClient)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Console.WriteLine("Нажмите ESC для выхода");
                Console.WriteLine("Сервер ждет сообщение от клиента");

                while (true)
                {
                    new Task(() =>
                    {
                        if (Console.ReadKey().Key == ConsoleKey.Escape) cts.Cancel();

                        if (token.IsCancellationRequested)
                        {
                            cts.Dispose();
                            Process.GetCurrentProcess().Kill();
                        }
                    }, token).Start();

                    Task.Run(() =>
                    {
                        var message = Listen();
                        var typeSend = _manager.Execute(message, _ipEndpoint);

                        Send(typeSend, message);

                        message?.Print();

                        SendAnswer(message.Command, message.NickFrom);
                    });
                }
            }
        }
    }
}
