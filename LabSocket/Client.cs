using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace src
{
    public delegate void ClientDelegate(Client client);
    public delegate void MessageDelegate(string message, Client client);

    public class Client : IDisposable
    {
        public MessageDelegate reciveMessage = default!;
        public MessageDelegate reciveInfo = default!;
        public ClientDelegate endConnection = default!;
        public string? ClientName { get { return (_socket?.RemoteEndPoint as IPEndPoint)?.Address.ToString(); } }
        protected Socket _socket;
        protected Task? _task = null;


        public Client(Socket? socket = null)
        {
            _socket = socket ?? new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public virtual void Start(string? addres = null)
        {
            _task = _task ?? Task.Run(
            () =>
            {
                try
                {
                    if (!_socket.Connected && addres != null)
                    {
                        var s = addres.Split(':');
                        _socket.Connect(s[0], int.Parse(s[1]));
                    }

                    var buffer = new byte[1024];
                    // while (_socket.Connected) // nie działa
                    while (!(_socket.Poll(100, SelectMode.SelectRead) && (_socket.Available == 0)))
                    {
                        int n = _socket.Receive(buffer);
                        reciveMessage(Encoding.UTF8.GetString(buffer, 0, n), this);
                    }
                }
                catch (Exception ex)
                {
                    reciveInfo($"<Error>: {ex.Message}", this);
                }
                endConnection(this);
                _task = null;
            });
        }

        public void Send(string message)
        {
            try
            {
                _socket.Send(Encoding.UTF8.GetBytes(message));
            }
            catch (Exception ex)
            {
                reciveInfo($"<Error>: {ex.Message}", this);
            }
        }

        public void Dispose()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            _task?.Wait();
        }
    }
}