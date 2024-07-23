using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] PlayerAnimationManager _playerAnimationManager;
    [SerializeField] PlayerInputManager _playerInputManager;

    public PlayerAnimationManager GetPlayerAnimationManager()
    {
        return _playerAnimationManager;
    }

    public PlayerInputManager GetPlayerInputManager()
    {
        return _playerInputManager;
    }
}
