using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] PlayerAnimationManager _playerAnimationManager;
    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] PlayerObjectUIManager _playerObjectUIManager;
    [SerializeField] string playername;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            if(!isLocalPlayer) return;
            SaySomething("YOU CAN HANDLE EVERYTHING!!");
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
}
