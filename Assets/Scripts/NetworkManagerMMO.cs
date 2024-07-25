using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class NetworkManagerMMO : NetworkManager
{
    public static new NetworkManagerMMO singleton => (NetworkManagerMMO)NetworkManager.singleton;
    [Scene] public string firstSceneToLoad;
    [Scene] public string[] subScenes;
    bool subscenesLoaded;

    private bool isInTransition;
    private bool firstSceneLoaded;

    public struct TestMessage : NetworkMessage
    {
        public string PlayerName;
        public int PlayerHealth;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<TestMessage>(OnClientTestMessageSend);
        StartCoroutine(LoadAllSubScenes());
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        StartCoroutine(UnloadScenes());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkClient.RegisterHandler<TestMessage>(OnServerTestMessageResponse);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        if (mode == NetworkManagerMode.Offline)
            StartCoroutine(UnloadScenes());
    }

    IEnumerator LoadAllSubScenes()
    {
        foreach (string scene in subScenes)
        {
            yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }

    IEnumerator UnloadScenes()
    {
        Debug.Log("Unloading Subscenes");

        foreach (string sceneName in subScenes)
            if (SceneManager.GetSceneByName(sceneName).IsValid() || SceneManager.GetSceneByPath(sceneName).IsValid())
                yield return SceneManager.UnloadSceneAsync(sceneName);

        yield return Resources.UnloadUnusedAssets();
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
