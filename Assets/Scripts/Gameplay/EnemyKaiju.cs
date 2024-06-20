using UnityEngine;

public class EnemyKaiju : Kaiju
{
    private EnemyKaijuSwap _kaijuSwap;
    
    private void OnEnable()
    {
        _battleMenuController.UpdateEnemyHealthBar(CurrentHealth, LocalHealth);
    }
    
    protected override void TakeDamage(float damageToDeal, MoveStats movePerformed)
    {
        _targetKaiju = FindFirstObjectByType<PlayerKaiju>();
        base.TakeDamage(damageToDeal, movePerformed);
    }

    public override void Attack(Kaiju targetKaiju, int moveToPerformIndex)
    {
        moveToPerformIndex = Random.Range(0, LearnedMoves.Length);
        base.Attack(targetKaiju, moveToPerformIndex);
    }
    
    protected override void Die()
    {
        base.Die();
        
        AddToPlayerXpProgression(_targetKaiju, _localXp);
        _kaijuSwap ??= FindFirstObjectByType<EnemyKaijuSwap>();
        _kaijuSwap.SwapInRandomKaiju();
    }
    
    private void AddToPlayerXpProgression(Kaiju playerKaiju, int xpToAdd)
    {
        playerKaiju.levelProgression += xpToAdd;
        _statusHandler.DisplayXpGain(xpToAdd);
        if (playerKaiju.levelProgression < playerKaiju.nextXp) return;
        
        var remainder = playerKaiju.levelProgression - playerKaiju.nextXp;
        playerKaiju.Level++;
        _statusHandler.DisplayLevelUpInformation(playerKaiju, playerKaiju.Level);
        _battleMenuController.RenewPlayerStatValues();
        playerKaiju.levelProgression = remainder;
        nextXp = KaijuStats.BaseXP * Level;
    }
}
