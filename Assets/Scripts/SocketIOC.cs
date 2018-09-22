using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;

public class Message
{
    public string Type;
    public MessageData Data;
}

public class MessageData
{
    public string Player;
    public double X;
    public double Y;
    public bool Flip;
    public bool Jump;
    public string Color;
}

public class QueueSocket : WebSocketBehavior
{
    private Queue<string> messageQueue;

    public QueueSocket(Queue<string> messageQueue)
    {
        this.messageQueue = messageQueue;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        messageQueue.Enqueue(e.Data);
    }
}

public class SocketIOC : MonoBehaviour
{
    public WebSocketServer socket;

    private Queue<string> messageQueue;

    void Start()
    {
        messageQueue = new Queue<string>();

        socket = new WebSocketServer(8080);
        socket.AddWebSocketService("/", () => new QueueSocket(messageQueue));
        socket.Start();
    }

    void Update()
    {
        for (int i = 0; i < messageQueue.Count; i++)
        {
            string json = messageQueue.Dequeue();
            Message message = JsonConvert.DeserializeObject<Message>(json);
            switch (message.Type)
            {
                case "join":
                    GetComponent<GameManager>().spawnPlayer(message.Data.Player, message.Data.Color);
                    break;
                case "leave":
                    GetComponent<GameManager>().deSpawnPlayer(message.Data.Player);
                    break;
                case "move":
                    Vector2 direction = new Vector2(
                        Convert.ToSingle(message.Data.Y),
                        Convert.ToSingle(message.Data.X)
                    );
                    Debug.Log(json);
                    GetComponent<GameManager>().movePlayer(message.Data.Player, direction, message.Data.Flip, message.Data.Jump);

                    break;
            }
        }
    }

    void OnDestroy()
    {
        socket.Stop();
    }
}

//If a player disconnects he will be despawned but his trails won't.