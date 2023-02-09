using System.Text;
using TestingProgram.Model.ClientServer.Protocol;

namespace ConsoleDebugTest.Model.ClientServer.Protocol
{
	class MyPacketConvector : IPacketConvector
	{
		public byte[] GetBytes(Packet packet)
		{
			string s = "";
			if (packet.Type == Packet.TypePacket.test)
				s = 't' + packet.Message;
			else s = 'c' + packet.Message;
			return Encoding.UTF32.GetBytes(s); 
		}

		public Packet GetPacket(byte[] buffer)
		{
			string s = Encoding.UTF32.GetString(buffer);
			return new Packet(s.StartsWith('t') ? Packet.TypePacket.test: Packet.TypePacket.command, s.Substring(1));
		}
	}
}
