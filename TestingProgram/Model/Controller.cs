using ConsoleDebugTest.Model.ClientServer.Protocol;
using System.IO;
using TestingProgram.Model.ClientServer.Protocol;
using TestingProgram.Model.ClientServer.Server;
using TestingProgram.Model.Testing;
using TestingProgram.Model.Testing.TestConverters;
using static TestingProgram.Model.ClientServer.Server.Server;

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
		public static Test SelectedTest { get; private set; }
		private static IPacketConvector packetConvector = new MyPacketConvector();
		private static ITestConverter testConvector = new STSTConvertor();

		public static bool StartServer(int port)
		{
			if (type != Type.None) return false;
			if (Server.Start(port))
			{
				Server.ClientMessage += Server_ClientMessage;
				type = Type.Server;
				return true;
			}
			return false;
		}

		private static void Server_ClientMessage(Client client, byte[] buffer)
		{
			Packet packet = packetConvector.GetPacket(buffer);
			if (packet.Type == Packet.TypePacket.test)
			{
				SelectedTest = testConvector.GetTest(packet.Message);
				StartTesting(SelectedTest);
			}
			else if (packet.Type == Packet.TypePacket.command)
			{
				switch (packet.Message)
				{
					case "STOP_TEST":
						StopTestingToServer?.Invoke();
						break;
					default:
						AddAnswer(new AnswerForTest(packet.Message));
						break;
				}
			}
		}

		public static bool Connect(string ip, int port)
		{
			if (type != Type.None) return false;
			if (ClientServer.Client.Client.Connect(ip, port))
			{
				type = Type.Client;
				ClientServer.Client.Client.NewMessage += Client_NewMessage;
				return true;
			}
			return false;
		}

		private static void Client_NewMessage(byte[] buffer)
		{
			Packet packet = packetConvector.GetPacket(buffer);
			if (packet.Type == Packet.TypePacket.test)
			{
				SelectedTest = testConvector.GetTest(packet.Message);
				StartTesting(SelectedTest);
			}
			else if (packet.Type == Packet.TypePacket.command)
			{
				switch (packet.Message)
				{
					case "STOP_TEST":
						StopTestingToClient?.Invoke();
						break;
					default:
						break;
				}
			}
		}


		public static void AddAnswer(AnswerForTest answer)
		{
			switch (type)
			{
				case Type.Server:
					GetAnswerToServer?.Invoke(answer);
					break;
				case Type.Client:
					Packet packet = new Packet(Packet.TypePacket.command, answer.ToString());
					ClientServer.Client.Client.Send(packetConvector.GetBytes(packet));
					break;
				case Type.None:
					break;
			}
		}
		public static void StartTesting(Test test)
		{
			switch (type)
			{
				case Type.Server:
					byte[] p = packetConvector.GetBytes(new Packet(Packet.TypePacket.test, testConvector.GetText(test)));
					for (int i = 0; i < Server.Clients.Length; i++)
					{
						Clients[i].Send(p);
					}
					
					StartTestingToServer?.Invoke(test);
					break;
				case Type.Client:
					StartTestingToClient?.Invoke(test);
					break;
			}
		}
		public static bool TestSave(Test test, string path)
		{
			if (!File.Exists(path))
			{
				TestToFile.SaveTest(testConvector, test, path);
				return true;
			}
			else return false;
		}
		public static void EndTesting()
		{
			switch (type)
			{
				case Type.Server:
					byte[] p = packetConvector.GetBytes(new Packet(Packet.TypePacket.test, "STOP_TEST"));
					for (int i = 0; i < Server.Clients.Length; i++)
						Clients[i].Send(p);
					break;
				case Type.Client:
					ClientServer.Client.Client.Send(packetConvector.GetBytes(new Packet(Packet.TypePacket.test, "STOP_TEST")));
					break;
				case Type.None:
					break;
			}
		}
		#region events
		public delegate void startTesting(Test test);
		public static event startTesting StartTestingToClient;
		public static event startTesting StartTestingToServer;

		public delegate void stopTesting();
		public static event stopTesting StopTestingToClient;
		public static event stopTesting StopTestingToServer;

		

		public delegate void getAnswerToServer(AnswerForTest answer);
		public static event getAnswerToServer GetAnswerToServer;
		#endregion
	}
}