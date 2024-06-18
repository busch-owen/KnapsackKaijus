using System;
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

    public void DetermineFirstKaiju(Kaiju playerKaiju, EnemyKaiju enemyKaiju, int playerMoveIndex)
    {
        AttackerTwoTurn = false;
        if (playerKaiju.LocalSpeed > enemyKaiju.LocalSpeed)
        {
            _moveCastIndex = playerMoveIndex;
            _firstKaiju = playerKaiju;
            _secondKaiju = enemyKaiju;
            playerKaiju.Attack(enemyKaiju, playerMoveIndex);
            Debug.Log("Player Went First");
        }
        else
        {
            //This is just temporary, the attack on K2 will be a random choice from the AI
            _moveCastIndex = playerMoveIndex;
            _firstKaiju = enemyKaiju;
            _secondKaiju = playerKaiju;
            enemyKaiju.Attack(playerKaiju, playerMoveIndex);
            Debug.Log("Enemy Went First");
        }
    }

    public void SecondTurn()
    {
        _statusHandler.ClearDetails();
        if (!_secondKaiju) return;
        AttackerTwoTurn = true;
        _secondKaiju.Attack(_firstKaiju, _moveCastIndex);
        StartCoroutine(_statusHandler.DisplayDetails());
        Debug.Log("Second attacker now attacking");
    }
}
