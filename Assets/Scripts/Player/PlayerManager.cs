using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using static NetworkManagerMMO;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] PlayerLocomotionManager _playerLocomotionManager;
    [SerializeField] PlayerAnimationManager _playerAnimationManager;
    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] PlayerObjectUIManager _playerObjectUIManager;
    [SerializeField] PlayerColorManager _playerColorManager;
    [SerializeField] NetworkManagerMMO _networkManagerMMO;
    public GameObject Canvas;
    [SerializeField] string playername;

    public static Action<PlayerManager> OnClientConnectToServer;

    private void Awake()
    {
        _networkManagerMMO = FindObjectOfType<NetworkManagerMMO>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
            OnClientConnectToServer?.Invoke(this);

        //NetworkIdentity networkIdentity = Canvas.GetComponent<NetworkIdentity>();
        // networkIdentity.AssignClientAuthority(connectionToClient);
    }

    public PlayerAnimationManager GetPlayerAnimationManager()
    {
        return _playerAnimationManager;
    }

    public PlayerInputManager GetPlayerInputManager()
    {
        return _playerInputManager;
    }

    public PlayerObjectUIManager GetPlayerObjectUIManager()
    {
        return _playerObjectUIManager;
    }

    public PlayerColorManager GetPlayerColorManager()
    {
        return _playerColorManager;
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            SaySomething("YOU CAN HANDLE EVERYTHING!!");
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TestMessage msg = new TestMessage
            {
                PlayerName = playername,
                PlayerHealth = 100
            };


            NetworkManagerMMO.singleton.SendTestMessageToServer(msg);

        }

    }

    [Command]
    public void SaySomething(string txt)
    {
        Debug.Log($"Hi I am server. I say you {txt}");

        SaySomethingRpc(txt);
    }

    [ClientRpc]
    public void SaySomethingRpc(string txt)
    {
        Debug.Log($"Client name : {playername}");
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("trigger on server with player and object");
        if (other.TryGetComponent<IPortal>(out IPortal portal))
        {
            _playerLocomotionManager.enabled = false;
            portal.ChangeScene(GetComponent<NetworkIdentity>(), _networkManagerMMO, this.gameObject);
            Debug.Log("trigger with portal on server");
        }
    }
}
