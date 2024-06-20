using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerInputDisabled { get; private set; }

    public delegate void OnPlayerInputStateChanged(bool currentState);

    public event OnPlayerInputStateChanged PlayerInputState;

    private void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.OnEncounter += EnterBattle;
        player.OnEnterTrainerView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponent<Trainer>();
            if (trainer != null) { StartCoroutine(trainer.TriggerTrainerBattle(player)); }
        };
    }

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

    void EnterBattle()
    {
        // 
    }
}
