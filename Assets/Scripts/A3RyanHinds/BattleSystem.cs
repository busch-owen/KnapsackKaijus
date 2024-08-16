using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerBattler;
    [SerializeField] BattleHUD playerHud;
    [SerializeField] BattleUnit enemyBattler;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] BattleDialogue battleDialogue;

    void Awake()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        playerBattler.SetupBattle();
        playerHud.SetKaijuData(playerBattler.Kaiju);
        enemyBattler.SetupBattle();
        enemyHUD.SetKaijuData(enemyBattler.Kaiju);

        StartCoroutine(battleDialogue.TypeText($"A wild {enemyBattler.Kaiju.KaijuBase.KaijuName} Appeared!"));
    }
}
