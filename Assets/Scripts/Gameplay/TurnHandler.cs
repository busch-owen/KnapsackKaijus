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
        if (playerKaiju.LocalSpeed > enemyKaiju.LocalSpeed) // Player's move
        {
            _moveCastIndex = playerMoveIndex;
            _firstKaiju = playerKaiju;
            _secondKaiju = enemyKaiju;
            playerKaiju.Attack(enemyKaiju, playerMoveIndex);
        }
        else if(enemyKaiju.LocalHealth > playerKaiju.LocalSpeed) // Enemy's move
        {
            _moveCastIndex = playerMoveIndex;
            _firstKaiju = enemyKaiju;
            _secondKaiju = playerKaiju;
            enemyKaiju.Attack(playerKaiju, playerMoveIndex);
        }
        else
        {
            var whoGoes = Random.Range(0, 2);
            switch (whoGoes)
            {
                case 0: // Player's move
                    _moveCastIndex = playerMoveIndex;
                    _firstKaiju = playerKaiju;
                    _secondKaiju = enemyKaiju;
                    playerKaiju.Attack(enemyKaiju, playerMoveIndex);
                    break;
                case 1: // Enemy's move
                    _moveCastIndex = playerMoveIndex;
                    _firstKaiju = enemyKaiju;
                    _secondKaiju = playerKaiju;
                    enemyKaiju.Attack(playerKaiju, playerMoveIndex);
                    break;
            }
        }
    }

    public void ForfeitMove(Kaiju playerKaiju, EnemyKaiju enemyKaiju)
    {
        AttackerTwoTurn = false;
        _firstKaiju = playerKaiju;
        _secondKaiju = enemyKaiju;
    }


    public void SecondTurn()
    {
        if (!_secondKaiju) return;
        AttackerTwoTurn = true;
        _statusHandler.ClearDetails();
        _secondKaiju.Attack(_firstKaiju, _moveCastIndex);
        StartCoroutine(_statusHandler.DisplayDetails());
    }
}
