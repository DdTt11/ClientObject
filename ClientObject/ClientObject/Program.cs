using System.Net;
using System.Net.Sockets;
 
IPAddress host = IPAddress.Loopback;
int port = 8888;
using TcpClient client = new TcpClient();
StreamReader? reader = null;
StreamWriter? writer = null;
 
try
{
    client.Connect(host, port);
    reader = new StreamReader(client.GetStream());
    writer = new StreamWriter(client.GetStream());
    Task.Run(()=>ReceiveMessageAsync(reader));
    await SendMessageAsync(writer);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
writer?.Close();
reader?.Close();

async Task SendMessageAsync(StreamWriter streamWriter)
{
    while (true)
    {
        string? message = Console.ReadLine();
        await streamWriter.WriteLineAsync(message);
        await streamWriter.FlushAsync();
    }
}

async Task ReceiveMessageAsync(StreamReader streamReader)
{
    while (true)
    {
        try
        {
            string? message = await streamReader.ReadLineAsync();
            if (string.IsNullOrEmpty(message)) continue;
            Console.WriteLine("Клиент получил " + DateTime.Now + ": " + message);
        }
        catch
        {
            break;
        }
    }
}