using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopPortal : NetworkBehaviour, IPortal
{
    [Scene]
    public string TargetScene;
    public string scenePosToSpawnOn;
    public void ChangeScene(NetworkIdentity networkIdentity, NetworkManagerMMO networkManagerMMO, GameObject player)
    {
        //SceneMessage message = new SceneMessage { sceneName = subScene, sceneOperation = SceneOperation.LoadAdditive };
        //networkIdentity.connectionToClient.Send(message);
        StartCoroutine(SendPlayerToNewScene(networkIdentity, networkManagerMMO, player));
    }

    IEnumerator SendPlayerToNewScene(NetworkIdentity networkIdentity, NetworkManagerMMO networkManagerMMO, GameObject player)
    {
        if(networkIdentity == null) yield return null;
        
        NetworkConnectionToClient conn = networkIdentity.connectionToClient;
        if(conn == null) yield break;

        conn.Send(new SceneMessage { sceneName = gameObject.scene.path, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true});

        yield return new WaitForSeconds(1);

        NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Unspawn);

        NetworkStartPosition[] allStartPos = FindObjectsOfType<NetworkStartPosition>();

        Transform start = networkManagerMMO.GetStartPosition();
        foreach (var item in allStartPos)
        {
            if(item.gameObject.scene.name == Path.GetFileNameWithoutExtension(TargetScene) && item.name == scenePosToSpawnOn) //
            {
                start = item.transform;
            }
        }

        player.transform.position = start.position;

        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByPath(TargetScene));

        conn.Send(new SceneMessage { sceneName = TargetScene, sceneOperation = SceneOperation.LoadAdditive, customHandling = true});

        NetworkServer.AddPlayerForConnection(conn, player);
        if(NetworkClient.localPlayer != null && NetworkClient.localPlayer.TryGetComponent<PlayerLocomotionManager>(out PlayerLocomotionManager playerLocomotionManager))
        {
            playerLocomotionManager.enabled = true;
        }
    }

}
