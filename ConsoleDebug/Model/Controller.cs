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
		private static IPacketConvector packetConvector = new JSONPacketConvector();
        private static ITestConverter testConvector = new STSTConvertor();

        public static void StartServer(int port)
		{
			if (type != Type.None) return;
			type = Type.Server;
			Server.Start(port);
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
			if (packet.Type == Packet.TypePacket.test)
			{
				SelectedTest = testConvector.GetTest(packet.Message);
			}
			else if(packet.Type == Packet.TypePacket.command)
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
			switch (type)
			{
				case Type.Server:
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
				TestToFile.SaveTest(new STSTConvertor(), test, path);
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
        #endregion
    }
}
