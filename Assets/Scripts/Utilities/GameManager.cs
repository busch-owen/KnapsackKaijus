using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerInputDisabled { get; private set; }

    public delegate void OnPlayerInputStateChanged(bool currentState);

    public event OnPlayerInputStateChanged PlayerInputState;

    public void DisablePlayerInput()
    {
        IsPlayerInputDisabled = true;
        PlayerInputState?.Invoke(IsPlayerInputDisabled);
    }

    public void EnablePlayerInput()
    {
        IsPlayerInputDisabled = false;
        PlayerInputState?.Invoke(IsPlayerInputDisabled);
    }
}
