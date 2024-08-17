using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerBattler;
    [SerializeField] BattleHUD playerHud;
    [SerializeField] BattleUnit enemyBattler;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogue battleDialogue;

    BattleState battleState;
    int _currentAction = 0;

    Vector2 _navDir;

    void Awake()
    {
        BattleMenu.MenuNav += NavigateMenu;
        BattleMenu.OnMenuSelected += HandleAButton; 
    }

    void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        playerBattler.SetupBattle();
        playerHud.SetKaijuData(playerBattler.Kaiju);
        enemyBattler.SetupBattle();
        enemyHUD.SetKaijuData(enemyBattler.Kaiju);

        yield return battleDialogue.TypeText($"A wild {enemyBattler.Kaiju.KaijuBase.KaijuName} Appeared!");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()
    {
        battleState = BattleState.PlayerAction;
        StartCoroutine(battleDialogue.TypeText("Choose an action"));
        battleDialogue.EnablActionSelector(true);
        battleDialogue.UpdateActionSelector(_currentAction);
    }

    void PlayerMove()
    {
        battleState = BattleState.PlayerMoveState;
        battleDialogue.EnableFightMenu(false);
        battleDialogue.EnableDialogueText(false);
        battleDialogue.EnableMoveSelector(true);
        
    }

    void NavigateMenu(Vector2 navDir)
    {
        _navDir = navDir;
        if (_navDir.y > 0f)
        {
            _currentAction++;
        }
        else if (_navDir.y < 0f)
        {
            _currentAction--;
        }

        if (_currentAction % battleDialogue.actionTexts.Count == 0)
        {
            _currentAction = 0;
        }
        else if (_currentAction < 0)
        {
            _currentAction = battleDialogue.actionTexts.Count;
        }
    }

    void HandleAButton()
    {
        if (_currentAction == 0)
        {
            PlayerMove();
        }
        else if (_currentAction == 1)
        {
            // Run
        }
    }
}

public enum BattleState { Start, PlayerAction, PlayerMoveState, EnemyMoveState, Busy }
