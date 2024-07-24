using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerMMO : NetworkManager
{
    public static new NetworkManagerMMO singleton => (NetworkManagerMMO)NetworkManager.singleton;

    public struct TestMessage : NetworkMessage
    {
        public string PlayerName;
        public int PlayerHealth;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<TestMessage>(OnClientTestMessageSend);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkClient.RegisterHandler<TestMessage>(OnServerTestMessageResponse);
    }

    void OnClientTestMessageSend(NetworkConnectionToClient conn, TestMessage message)
    {
        Debug.Log($"Hello I am server. I recieved a message. This is player name {message.PlayerName} and his health {message.PlayerHealth}");
        conn.Send(message);
    }

    public void OnServerTestMessageResponse(TestMessage message)
    {
        Debug.Log($"Hello I am client. I recieved a response. This is player name {message.PlayerName} and his health {message.PlayerHealth}");
    }

    public void SendTestMessageToServer(TestMessage msg)
    {
        NetworkClient.Send(msg);
    }
}
