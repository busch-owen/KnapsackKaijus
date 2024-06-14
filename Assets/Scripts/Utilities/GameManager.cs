using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerInputDisabled { get; private set; }
    public event Action OnPlayerInputStateChanged;

    public void DisablePlayerInput()
    {
        IsPlayerInputDisabled = true;
        OnPlayerInputStateChanged?.Invoke();
    }

    public void EnablePlayerInput()
    {
        IsPlayerInputDisabled = false;
        OnPlayerInputStateChanged?.Invoke();
    }
}
