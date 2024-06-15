using System;
using Unity.VisualScripting;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    private Kaiju _firstKaiju;
    private Kaiju _secondKaiju;

    private int _moveCastIndex;

    private RoundStatusHandler _statusHandler;
    
    public bool AttackerTwoTurn { get; private set; }

    private void Awake()
    {
        _statusHandler = FindFirstObjectByType<RoundStatusHandler>();
    }

    public void DetermineFirstKaiju(Kaiju k1, Kaiju k2, int playerMoveIndex)
    {
        AttackerTwoTurn = false;
        if (k1.LocalSpeed > k2.LocalSpeed)
        {
            _moveCastIndex = playerMoveIndex;
            _firstKaiju = k1;
            _secondKaiju = k2;
            k1.Attack(k2, playerMoveIndex);
            Debug.Log("Player Went First");
        }
        else
        {
            //This is just temporary, the attack on K2 will be a random choice from the AI
            _moveCastIndex = playerMoveIndex;
            _firstKaiju = k2;
            _secondKaiju = k1;
            k2.Attack(k1, playerMoveIndex);
            Debug.Log("Enemy Went First");
        }
    }

    public void SecondTurn()
    {
        AttackerTwoTurn = true;
        _secondKaiju.Attack(_firstKaiju, _moveCastIndex);
        StartCoroutine(_statusHandler.DisplayDetails());
        Debug.Log("Second attacker now attacking");
    }
}
