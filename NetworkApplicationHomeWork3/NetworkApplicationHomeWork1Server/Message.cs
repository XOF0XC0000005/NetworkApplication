using System.Text.Json;

namespace NetworkApplicationHomeWork1Server
{
    public class Message
    {
        public Commands Command { get; set; }
        public string? Text { get; set; }
        public DateTime DateTime { get; set; }
        public string? NickFrom { get; set; }
        public string? NickTo { get; set; }

        public string SerializeMessageToJson() => JsonSerializer.Serialize(this);
        public static Message? DeserializeMessageFromJson(string message) => JsonSerializer.Deserialize<Message>(message);
        public void Print() => Console.WriteLine(ToString());
        public override string ToString() => $"{DateTime} получено сообщение {Text} от {NickFrom}";
    }
}