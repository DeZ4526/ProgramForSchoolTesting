namespace TestingProgram.Model.ClientServer.Protocol
{
	public class JSONPacketConvector : IPacketConvector
	{
		public byte[] GetBytes(Packet packet)
		{
			return new byte[1];
		}

		public Packet GetPacket(byte[] buffer)
		{
			return new Packet(Packet.TypePacket.command, "HELLO");
		}
	}
}
