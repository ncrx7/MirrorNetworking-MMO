using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ShopPortal : NetworkBehaviour, IPortal
{
    [Scene]
    public string subScene;
    public void ChangeScene(NetworkIdentity networkIdentity)
    {
        SceneMessage message = new SceneMessage { sceneName = subScene, sceneOperation = SceneOperation.LoadAdditive };
        networkIdentity.connectionToClient.Send(message);
    }


}
