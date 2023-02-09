using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TestingProgram.Model.ClientServer.Server
{
	class Client
	{
		public string LocalEndPoint { get => _socket.LocalEndPoint.ToString(); }
		public string RemoteEndPoint { get => _socket.RemoteEndPoint.ToString(); }
		private readonly Socket _socket;
		private bool _canSend = true;

		public Client(Socket socket)
		{
			_socket = socket;
			Reading();
		}
		private async void Reading()
		{
			byte[] bufLength = new byte[6]; //буфер для приема длинны
			byte[] bufAll; //буфер для приема всей информации
			byte[] buf = new byte[256]; //буфер для приема
			int size = 0; //сколько пришло
			int sizeZ = 0; //положение каретки
			int _length = 0; //длинна сообщения
			await Task.Run(() =>
			{
				while (IsConnected())
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
									Server.Disconnect(this);
									return;
								}
								sizeZ = size;
							}
							Server.Receive(this, bufAll);
						}
						bufAll = Array.Empty<byte>();//обнуляем буфер, дабы память не засорять
					}
					catch
					{
						break;
					}
				}
				Server.Disconnect(this);
			});
		}
		public bool Send(byte[] buffer)
		{
			if (_canSend)
			{
				_canSend = false;
				byte[] bufLen = new byte[6];
				try
				{
					Array.Copy(BitConverter.GetBytes(buffer.Length), bufLen, 4);
					bufLen[4] = 45;
					bufLen[5] = 26;
					_socket.Send(bufLen, 6, SocketFlags.None);
					_socket.Send(buffer, buffer.Length, SocketFlags.None);
					_canSend = true;
					return true;
				}
				catch
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		public bool IsConnected()
		{
			try
			{
				return !(_socket.Poll(1, SelectMode.SelectRead) && _socket.Available == 0);
			}
			catch (SocketException)
			{
				return false;
			}
		}
		
	}
}
