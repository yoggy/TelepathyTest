using System;
using System.Threading.Tasks;

class Client
{
    static Telepathy.Client client;

    static void Main(string[] args)
    {
        client = new Telepathy.Client();
        client.Connect("localhost", 1337);

        Task.Run(() =>
        {
            while(client.Connected)
            {
                Telepathy.Message msg;
                while (client.GetNextMessage(out msg))
                {
                    // clientの場合、msg.connectionIdは常に0が格納されている
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            Console.WriteLine("Telepathy.EventType.Connected : msg.connectionId=" + msg.connectionId);
                            break;
                        case Telepathy.EventType.Data:
                            Console.WriteLine("Telepathy.EventType.Data : msg.connectionId=" + msg.connectionId + ", msg.data" + System.Text.Encoding.UTF8.GetString(msg.data));
                            break;
                        case Telepathy.EventType.Disconnected:
                            Console.WriteLine("Telepathy.EventType.Disconnected : msg.connectionId=" + msg.connectionId);
                            break;
                    }
                }
            }
        });

        bool break_flag = false;
        while (break_flag == false)
        {
            ConsoleKey k = Console.ReadKey().Key;
            switch (k)
            {
                case ConsoleKey.Spacebar:
                    Console.WriteLine("send massage from client...");
                    client.Send(System.Text.Encoding.UTF8.GetBytes("send message from server..."));
                    break;
                case ConsoleKey.Escape:
                    break_flag = true;
                    break;
            }
        }

        client.Disconnect();
    }
}

