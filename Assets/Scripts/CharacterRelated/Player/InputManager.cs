using System;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerMover _playerMover;
    private InputMaster _input;
    
    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
    }

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new InputMaster();
            if (!GameManager.Instance.IsPlayerInputDisabled)
            {
                _input.Player.Move.started += ctx => _playerMover.ProcessMovement(ctx.ReadValue<Vector2>());
                _input.Player.Move.canceled += ctx => _playerMover.ProcessMovement(ctx.ReadValue<Vector2>());
            }
        }
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
