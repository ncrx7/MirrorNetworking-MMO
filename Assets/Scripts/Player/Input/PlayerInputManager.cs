using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : NetworkBehaviour
{
    public PlayerControl _playerControls;
    [SerializeField] public Vector2 _movementInput;
    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public float MoveAmount { get; private set; }

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControl();

            //BU EVENTE HANDLE MOVEMENT FONKSİYONU ENTEGRE EDİLİRSE DAHA İYİ OPTİMİZE EDİLİR
            _playerControls.PlayerMovement.Movement.performed += i => _movementInput = i.ReadValue<Vector2>();

        }

        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.PlayerMovement.Movement.performed -= i => _movementInput = i.ReadValue<Vector2>();
    }

    private void Update()
    {
        //Debug.Log("movement input : " + _movementInput);
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        HorizontalInput = _movementInput.x;
        VerticalInput = _movementInput.y;
        MoveAmount = Mathf.Clamp01(Mathf.Abs(VerticalInput) + Mathf.Abs(HorizontalInput));

        if (MoveAmount >= 0.5 && MoveAmount > 0)
        {
            MoveAmount = 0.5f;
        }
      
    }

}
