using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerController _playerMover;
    private InputMaster _input;
    private BattleMenu _battleMenu;

    Dictionary<GameInputType, InputActionMap> _actionmaps;
    
    private void Awake()
    {
        _playerMover = GetComponent<PlayerController>();
        _battleMenu = GetComponent<BattleMenu>();
    }

    private void OnEnable()
    {
        if (_actionmaps == null)
        {
            _actionmaps = new Dictionary<GameInputType, InputActionMap>();
        }
        if (_input == null)
        {
            _input = new InputMaster();
            // Adds the player movement to the inputmapping
            _input.Player.Move.started += ctx => _playerMover.ProcessMovement(ctx.ReadValue<Vector2>());
            _input.Player.Move.canceled += ctx => _playerMover.ProcessMovement(ctx.ReadValue<Vector2>());
            _actionmaps.Add(GameInputType.Player, _input.Player);
            // Adds the Battle menu navigation to the interactions.
            _input.Battle.Move.performed += (ctx) => _battleMenu.HandleMenuNav(ctx.ReadValue<Vector2>());
            _input.Battle.A.performed += (ctx) => _battleMenu.HandleAButton();
            _actionmaps.Add(GameInputType.Battle, _input.Battle);
        
        }
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    public void EnableInput(GameInputType inputType)
    {
        _actionmaps[inputType].Enable();
    }

    public void DisableInput(GameInputType inputType)
    {
        _actionmaps[inputType].Disable();
    }
}

public enum GameInputType { Player,  Battle }
