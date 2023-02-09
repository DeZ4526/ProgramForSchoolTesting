using System;
using System.Net.Sockets;
using System.Threading;

namespace TestingProgram.Model.ClientServer.Client
{
	static class Client
	{
		private static Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		private static readonly Thread _receive = new Thread(Receive);
		private static string _ip;
		private static int _port;
		public static bool _canSend = false;
		public static string Ip { get => _ip; }
		public static int Port { get => _port; }
		public static bool IsConnected { get => _socket.Connected; }


		public static bool Connect(string ip, int port)
		{
			_ip = ip;
			_port = port;
			try
			{
				_socket.Connect(_ip, _port);
				_receive.Start();
				_canSend = true;
				OnConnect?.Invoke();
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		public static bool Send(byte[] buffer)
		{
			if (_canSend)
			{
				_canSend = false;
				byte[] bufLen = new byte[6];
				try
				{
					Array.Copy(BitConverter.GetBytes(buffer.Length), bufLen,4);
					bufLen[4] = 45;
					bufLen[5] = 26;
					_socket.Send(bufLen, 6, SocketFlags.None);
					_socket.Send(buffer, buffer.Length, SocketFlags.None);
					_canSend = true;
					return true;
				}
				catch
				{
					Disconnect();
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		private static void Receive()
		{
			byte[] bufLength = new byte[6]; //буфер для приема длинны
			byte[] bufAll; //буфер для приема всей информации
			byte[] buf = new byte[256]; //буфер для приема
			int size = 0; //сколько пришло
			int sizeZ = 0; //положение каретки
			int _length = 0; //длинна сообщения

			while (IsConnected)
			{
				size = 0;
				sizeZ = 0;
				_length = 0;
				try
				{
					_socket.Receive(bufLength, 6, SocketFlags.None); //принимаем пакет с длинной и заголовком
					if (bufLength[4] == 45 && bufLength[5] == 26) //проверяем совпадает ли заголовок
					{
						_length = BitConverter.ToInt32(bufLength, 0); //копируем длинну
						bufAll = new byte[_length]; //резервируем место для основной информации
						while (size < _length) //пока размер принятых данных меньше намечанной длинны
						{
							size += _socket.Receive(buf, buf.Length, SocketFlags.None); //принимаем данные
							Array.Copy(buf, 0, bufAll, sizeZ, _length - sizeZ > 256 ? 256 : _length - sizeZ); //записываем их в буфер для приема и проверяем конец ли
							if (sizeZ == size) //если коретка не сдвигается, значит во время передачи связь прервалась
							{
								Disconnect();
								return;
							}
							sizeZ = size;
						}
						NewMessage?.Invoke(bufAll);
					}
					bufAll = Array.Empty<byte>();//обнуляем буфер, дабы память не засорять
				}
				catch
				{
					break;
				}
			}
			Disconnect();

		}
		public static void Disconnect()
		{
			try
			{
				_socket.Shutdown(SocketShutdown.Both);
				_socket.Close();
				_socket.Disconnect(true);
				_canSend = true;
			}
			catch
			{

			}
			OnDisconnect?.Invoke();
		}
		
		#region events
		public delegate void onConnect();
		public static event onConnect OnConnect;

		public delegate void onDisconnect();
		public static event onDisconnect OnDisconnect;

		public delegate void newMessage(byte[] buffer);
		public static event newMessage NewMessage;
		#endregion
	}
}
