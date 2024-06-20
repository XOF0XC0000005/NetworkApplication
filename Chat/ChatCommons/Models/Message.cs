using System.Net;
using System.Text.Json;

namespace ChatCommon.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public Command Command { get; set; }
        public string Text { get; set; }
        public DateTime DateSend { get; set; }
        public bool IsSent { get; set; }
        public IPEndPoint EndPoint { get; set; }
   
        public string NickNameTo {  get; set; }
        public string NickNameFrom {  get; set; }

        public string SerializeMessageToJson() => JsonSerializer.Serialize(this);
        public static Message? DeserializeMessageFromJson(string message) => JsonSerializer.Deserialize<Message>(message);
        public void Print() => Console.WriteLine(ToString());
        public override string ToString() => $"{DateSend} получено сообщение {Text} от {NickNameFrom}";
    }
}
