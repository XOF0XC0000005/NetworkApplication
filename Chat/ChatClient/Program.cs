namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.SentMessage(args[0], args[1]);
        }
    }
}
