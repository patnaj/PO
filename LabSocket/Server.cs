using System.Net;

namespace src
{
    public class Server : Client
    {
        private List<Client> _clients;
        public ClientDelegate newConnection = default!;
        public Server() : base(null)
        {
            _clients = new List<Client>();
        }

        public override void Start(string? addres = null)
        {
            _task = _task ?? Task.Run(
            () =>
            {
                try
                {
                    _socket.Bind(new IPEndPoint(IPAddress.Any, int.Parse((addres??"").Split(':').Last())));
                    _socket.Listen(100);
                    while (true)
                    {
                        var cs = _socket.Accept();
                        if (cs != null)
                        {
                            var client = new Client(cs);
                            _clients.Add(client);
                            client.reciveMessage += ResendMessage;
                            client.endConnection += RemoveClient;
                            client.reciveInfo += reciveInfo;
                            client.Start();
                            newConnection(client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    reciveInfo($"<Error>: {ex.Message}", this);
                }
                _task = null;
            });
        }

        protected void ResendMessage(string message, Client client)
        {
            _clients.ForEach(c => { if (c != client) c.Send(message); });
            reciveMessage(message, client);
        }

        protected void RemoveClient(Client client)
        {
            _clients.Remove(client);
            reciveInfo($"<Disconnect>", client);
        }
    }
}