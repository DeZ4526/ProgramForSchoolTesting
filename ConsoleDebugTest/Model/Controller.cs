using ConsoleDebugTest.Model.ClientServer.Protocol;
using ConsoleDebugTest.Model.Testing.TestConverters;
using System.IO;
using TestingProgram.Model.ClientServer.Protocol;
using TestingProgram.Model.ClientServer.Server;
using TestingProgram.Model.Testing;
using TestingProgram.Model.Testing.TestConverters;
using static System.Net.Mime.MediaTypeNames;
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

		public static void StartServer(int port)
		{
			if (type != Type.None) return;
			if (Server.Start(port))
			{
				Server.ClientMessage += Server_ClientMessage;
				type = Type.Server;
			}
		}

		private static void Server_ClientMessage(Client client, byte[] buffer)
		{
			Packet packet = packetConvector.GetPacket(buffer);
			Console.WriteLine(packet.Type);
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
						AddAnswer(new AnswerForTest(packet.Message));
						break;
				}
			}
		}

		public static void Connect(string ip, int port)
		{
			if (type != Type.None) return;
			if (ClientServer.Client.Client.Connect(ip, port))
			{
				type = Type.Client;
				ClientServer.Client.Client.NewMessage += Client_NewMessage;
			}
		}

		private static void Client_NewMessage(byte[] buffer)
		{
			Packet packet = packetConvector.GetPacket(buffer);
			Console.WriteLine(packet.Type);
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
					break;
				case Type.Client:
					break;
				case Type.None:
					break;
				default:
					break;
			}
		}
		#region events
		public delegate void startTestingToClient(Test test);
		public static event startTestingToClient StartTestingToClient;

		public delegate void stopTestingToClient();
		public static event stopTestingToClient StopTestingToClient;

		public delegate void startTestingToServer(Test test);
		public static event startTestingToServer StartTestingToServer;

		public delegate void getAnswerToServer(AnswerForTest answer);
		public static event getAnswerToServer GetAnswerToServer;
		#endregion
	}
}