using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionManager : NetworkBehaviour
{
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
            
        HandleMove();
    }

    private void HandleMove()
    {
        Vector2 direction = new Vector2(_playerManager.GetPlayerInputManager().HorizontalInput, _playerManager.GetPlayerInputManager().VerticalInput).normalized;
        _rigidbody.velocity = direction * _speed;

        SetMovementParameterOnMove();
    }

    private void SetMovementParameterOnMove()
    {
        _playerManager.GetPlayerAnimationManager().SetAnimatorValue(AnimatorParameterType.FLOAT, "horizontal", floatValue:_playerManager.GetPlayerInputManager().HorizontalInput);
        _playerManager.GetPlayerAnimationManager().SetAnimatorValue(AnimatorParameterType.FLOAT, "vertical", floatValue:_playerManager.GetPlayerInputManager().VerticalInput);
        _playerManager.GetPlayerAnimationManager().SetAnimatorValue(AnimatorParameterType.FLOAT, "Blend", floatValue:_playerManager.GetPlayerInputManager().MoveAmount);
    }
}
