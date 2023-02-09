namespace TestingProgram.Model.ClientServer.Protocol
{
	public interface IPacketConvector
	{
		Packet GetPacket(byte[] buffer);
		byte[] GetBytes(Packet packet);

	}
}
