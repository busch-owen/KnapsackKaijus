using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerController _playerMover;
    private InputMaster _input;
    
    private void Awake()
    {
        _playerMover = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new InputMaster();
            if (GameManager.Instance.GameState == GameState.ROAM)
            {
                _input.Player.Move.started += ctx => _playerMover.ProcessMovement(ctx.ReadValue<Vector2>());
                _input.Player.Move.canceled += ctx => _playerMover.ProcessMovement(ctx.ReadValue<Vector2>());
                _input.Player.Start.performed += ctx => GameManager.Instance.PlayerInputState += OnPlayerInputStateChanged;
            }
        }
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
