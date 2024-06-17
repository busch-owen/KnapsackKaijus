public class EnemyKaiju : Kaiju
{

    protected override void TakeDamage(float damageToDeal, MoveStats movePerformed)
    {
        _targetKaiju = FindFirstObjectByType<PlayerKaiju>();
        base.TakeDamage(damageToDeal, movePerformed);
    }
    
    protected override void Die()
    {
        AddToPlayerXpProgression(_targetKaiju, _localXp);
        base.Die();
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
