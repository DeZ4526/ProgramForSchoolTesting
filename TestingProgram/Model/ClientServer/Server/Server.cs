using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestingProgram.Model.ClientServer.Server
{
	static class Server
	{
		private static readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private static readonly List<Client> _clients = new List<Client>();
		private static readonly Thread _accepting = new Thread(Accepting);
		public static Client[] Clients { get => _clients.ToArray(); }

		private static int _port;
		public static int Port { get => _port; }
		public static bool Start(int port)
		{
			_port = port;
			try
			{
				_socket.Bind(new IPEndPoint(IPAddress.Any, port));
				_socket.Listen(10);
				_accepting.Start();
				ServerStart?.Invoke();
				return true;
			}
			catch (Exception ex)
			{
				ServerError?.Invoke(ex);
				return false;
			}
		}
		public static void Receive(Client client, byte[] buffer)
		{
			ClientMessage?.Invoke(client, buffer);
		}
		private static void Accepting()
		{
			while (true)
			{
				Client _newClient = new Client(_socket.Accept());
				_clients.Add(_newClient);
				ClientConnect?.Invoke(_newClient);
			}
		}
		public static void Disconnect(Client client)
		{
			ClientDisconnect?.Invoke(client);
			_clients.Remove(client);
		}
		#region events
		public delegate void clientConnect(Client client);
		public static event clientConnect ClientConnect;

		public delegate void clientDisconnect(Client client);
		public static event clientDisconnect ClientDisconnect;

		public delegate void clientMessage(Client client, byte[] buffer);
		public static event clientMessage ClientMessage;


		public delegate void serverError(Exception exception);
		public static event serverError ServerError;

		public delegate void serverStart();
		public static event serverStart ServerStart;

		#endregion
	}
}
