# TelepatyTest
https://github.com/vis2k/Telepathy

## Server.cs
```Server.cs
using System;
using System.Threading.Tasks;

class Server
{
    static Telepathy.Server server;

    static void Main(string[] args)
    {
        server = new Telepathy.Server();
        server.Start(1337);

        Task.Run(() =>
        {
            while(server.Active)
            {
                Telepathy.Message msg;
                while (server.GetNextMessage(out msg))
                {
                    switch (msg.eventType)
                    {
                        case Telepathy.EventType.Connected:
                            // msg.connectionIdはクライアント接続時に+1される数値。1始まり
                            Console.WriteLine("Telepathy.EventType.Connected : msg.connectionId=" + msg.connectionId);
                            break;
                        case Telepathy.EventType.Data:
                            Console.WriteLine("Telepathy.EventType.Data : msg.connectionId=" + msg.connectionId + ", msg.data" + System.Text.Encoding.UTF8.GetString(msg.data));

                            // reply
                            Console.WriteLine("send massage from server...");
                            server.Send(msg.connectionId, System.Text.Encoding.UTF8.GetBytes("send message from server..."));
                            break;
                        case Telepathy.EventType.Disconnected:
                            Console.WriteLine("Telepathy.EventType.Disconnected : msg.connectionId=" + msg.connectionId);
                            break;
                    }
                }
            }
        });

        bool break_flag = false;
        while (break_flag == false) {
            ConsoleKey k = Console.ReadKey().Key;
            switch (k)
            {
                case ConsoleKey.L:
                    break;
                case ConsoleKey.Escape:
                    break_flag = true;
                    break;
            }
        }

        server.Stop();
    }
}
```

## Client.cs
```Client.cs
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
```

## Copyright and license
Copyright (c) 2020 yoggy

Released under the [MIT license](LICENSE.txt)