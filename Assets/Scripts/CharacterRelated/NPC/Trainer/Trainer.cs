using System;
using UnityEngine;
using System.Collections;

public class Trainer : MonoBehaviour
{
    private void Awake()
    {
        // do some shit here
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        GameManager.Instance.GameState = GameState.BATTLE;
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Starting Battle!");
    }
}
