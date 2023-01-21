using TestingProgram.Model.ClientServer.Server;
using TestingProgram.Model.Testing;

namespace TestingProgram.Model
{
	static class Controller
	{
		enum Type : byte
		{
			Server,
			Client,
			None
		}
		private static Type type = Type.None;
		public static void StartServer(int port)
		{
			if (type != Type.None) return;
			type = Type.Server;
			Server.Start(port);
		}
		public static void Connect(string ip, int port)
		{
			if (type != Type.None) return;
			if(ClientServer.Client.Client.Connect(ip, port))
				type = Type.Client;
		}
		public static void AddAnswer()
		{
			switch (type)
			{
				case Type.Server:
					break;
				case Type.Client:
					break;
				case Type.None:
					break;
			}
		}
		public static void StartTesting(Test test)
		{

		}
		public static void EndTesting()
		{
			switch (type)
			{
				case Type.Server:
					break;
				case Type.Client:
					break;
				case Type.None:
					break;
				default:
					break;
			}
		}

	}
}
