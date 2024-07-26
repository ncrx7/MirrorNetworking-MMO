using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.IO;

public class NetworkManagerMMO : NetworkManager
{
    public static new NetworkManagerMMO singleton => (NetworkManagerMMO)NetworkManager.singleton;
    [SerializeField] private FadeInOutManager _fadeInOutManager;
    public string firstSceneToLoad;
    public string[] scenesToLoad;
    bool subscenesLoaded;

    private readonly List<Scene> subScenes = new List<Scene>();

    private bool isInTransition;
    private bool firstSceneLoaded;

    public struct TestMessage : NetworkMessage
    {
        public string PlayerName;
        public int PlayerHealth;
    }

    private void Start()
    {
        int sceneScount = SceneManager.sceneCountInBuildSettings - 2;
        scenesToLoad = new string[sceneScount];

        for (int i = 0; i < sceneScount; i++)
        {
            scenesToLoad[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i + 2));
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        _fadeInOutManager.ShowScreenNoDelay();

        if (sceneName == onlineScene)
        {
            StartCoroutine(LoadAllSubScenes());
        }
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        if(sceneOperation == SceneOperation.LoadAdditive)
        {
            StartCoroutine(LoadAdditive(newSceneName));
        }

        if(sceneOperation == SceneOperation.UnloadAdditive)
        {
            StartCoroutine(UnloadAdditive(newSceneName));
        }
    }

    public override void OnClientSceneChanged()
    {
        if (isInTransition == false)
            base.OnClientSceneChanged();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<TestMessage>(OnClientTestMessageSend);
        //StartCoroutine(LoadAllSubScenes());
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        //StartCoroutine(UnloadScenes());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkClient.RegisterHandler<TestMessage>(OnServerTestMessageResponse);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();


    }

    IEnumerator LoadAllSubScenes()
    {
        foreach (string scene in scenesToLoad)
        {
            yield return SceneManager.LoadSceneAsync(scene, new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Additive,
                localPhysicsMode = LocalPhysicsMode.Physics2D
            });
        }

        subscenesLoaded = true;
    }

    IEnumerator UnloadScenes()
    {
        Debug.Log("Unloading Subscenes");
        yield return null;
        /*         foreach (string sceneName in subScenes)
                    if (SceneManager.GetSceneByName(sceneName).IsValid() || SceneManager.GetSceneByPath(sceneName).IsValid())
                        yield return SceneManager.UnloadSceneAsync(sceneName);

                yield return Resources.UnloadUnusedAssets(); */
    }

    IEnumerator LoadAdditive(string sceneName)
    {
        isInTransition = true;
        yield return _fadeInOutManager.FadeIn();

        if (mode == NetworkManagerMode.ClientOnly)
        {
            loadingSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
            {
                yield return null;
            }
        }

        NetworkClient.isLoadingScene = false;
        isInTransition = false;

        OnClientSceneChanged();

        if (firstSceneLoaded == false)
        {
            firstSceneLoaded = true;
            yield return new WaitForSeconds(0.6f);
        }
        else
        {
            firstSceneLoaded = true;
            yield return new WaitForSeconds(0.5f);
        }

        yield return _fadeInOutManager.FadeOut();
    }

    IEnumerator UnloadAdditive(string sceneName)
    {
        isInTransition = true;
        yield return _fadeInOutManager.FadeIn();

        if(mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        NetworkClient.isLoadingScene = false;
        isInTransition = false;

        OnClientSceneChanged();
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

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        if(conn.identity == null)
        {
            StartCoroutine(AddPlayerDelayed(conn));
        }
    }

    IEnumerator AddPlayerDelayed(NetworkConnectionToClient conn)
    {
        while(!subscenesLoaded)
            yield return null;

        NetworkIdentity[] networkIdentities = FindObjectsOfType<NetworkIdentity>();

        foreach (var item in networkIdentities)
        {
            item.enabled = true;
        }

        firstSceneLoaded = false;

        conn.Send(new SceneMessage{sceneName = firstSceneToLoad, sceneOperation = SceneOperation.LoadAdditive, customHandling = true});

        Transform startPos = GetStartPosition();

        GameObject player = Instantiate(playerPrefab, startPos);
        player.transform.SetParent(null);

        yield return new WaitForEndOfFrame();

        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(firstSceneToLoad));

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
