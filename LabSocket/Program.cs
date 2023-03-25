using System.Linq;
using System.Linq.Expressions;
using src;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"To exit: Ctrl+C");
        var default_addres = "localhost:5000";
        Console.WriteLine($"addres ({default_addres})?");
        var addres = Console.ReadLine();
        if (addres == null || addres == "")
            addres = default_addres;
        Console.WriteLine("start server (y)?");
        var is_server = Console.ReadLine()?.Trim() != "n";


        Server server = default!;
        if (is_server)
        {
            server = new Server();
            server.newConnection += (cli) => { Console.WriteLine($"server - new user: {cli.ClientName}"); };
            server.reciveInfo += (mes, cli) => { Console.WriteLine($"server - info: {cli.ClientName} {mes}"); };
            server.reciveMessage += (mes, cli) => { Console.WriteLine($"server - message: {mes}"); };
            server.Start(addres);
            Console.WriteLine($"server - start on {addres}...");
        }

        var client = new Client();
        client.reciveMessage += (mes, cli) => { Console.WriteLine(mes); };
        client.endConnection += (cli) => { Console.WriteLine($"disconnect"); };
        client.Start(addres);


        bool run = true;
        Console.CancelKeyPress += (o, e) => { run = false; };
        while (run)
        {
            var mess = Console.ReadLine() ?? "";
            client.Send(mess);
            Console.WriteLine($"send {mess}");
        }
        Console.WriteLine($"EndProgram");
    }

}
