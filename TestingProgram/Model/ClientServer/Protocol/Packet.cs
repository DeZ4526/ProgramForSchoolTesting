
namespace TestingProgram.Model.ClientServer.Protocol
{
    public class Packet
    {
        public enum TypePacket
        {
            command,
            test
        }

        public TypePacket Type { get; set; } = TypePacket.command;

        public Packet(TypePacket type, string message)
        {
            Type = type;
            Message = message;
        }

        public string Message { get; set; }


    }
}
