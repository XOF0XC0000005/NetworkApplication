using ChatServer;

namespace ChatService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();
            server.Start();
        }
    }
}
