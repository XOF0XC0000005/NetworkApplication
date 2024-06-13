using System.Net;

namespace NetworkApplicationHomeWork1Server
{
    public class Manager
    {
        private Server _server;
        public Manager(Server server) => _server = server;
        public event EventHandler SendAnswer;

        public void SendAnswerToUser()
        {
            SendAnswer?.Invoke(this, EventArgs.Empty);
        }
        public TypeSend Execute(Message message, IPEndPoint iPEndpoint)
        {
            switch (message.Command)
            {
                case Commands.Delete:
                    Delete(message.NickFrom);
                    break;
                case Commands.Register:
                    Register(message.NickFrom, iPEndpoint);
                    break;
                case Commands.Message:
                    Send(message);
                    break;
            }
            return TypeSend.Default;
        }

        public TypeSend Send(Message message)
        {
            if (string.IsNullOrEmpty(message.NickTo))
                return TypeSend.ToAll;
            return TypeSend.ToOne;
        }

        public void Register(string user, IPEndPoint ipEndpoint)
        {
            if (_server.Users == null)
                _server.Users = new Dictionary<string, IPEndPoint>();
            if (!_server.Users.ContainsKey(user))
            {
                _server.Users.Add(user, ipEndpoint);
                Console.WriteLine($"Пользователь {user} зарегистрирован!");
            }
        }

        public void Delete(string user)
        {
            _server.Users.Remove(user);
            Console.WriteLine($"Пользователь {user} удален!");
        }
    }
}
