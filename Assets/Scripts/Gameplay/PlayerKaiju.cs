using System;
using UnityEngine;

public class PlayerKaiju : Kaiju
{
    private void OnEnable()
    {
        _battleMenuController.UpdatePlayerHealthBar(CurrentHealth, LocalHealth);
    }

    protected override void Die()
    {
        //Allow player to swap a new Kaiju into battle so long as there is another Kaiju to swap in
        base.Die();
        if (_battleMenuController.CheckIfAllDead())
        {
            return;
        }
        _battleMenuController.OpenKaijuMenuAfterDeath();
    }
}
