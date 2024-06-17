public class EnemyKaiju : Kaiju
{
    protected override void Die()
    {
        base.Die();
        AddToPlayerXpProgression(_targetKaiju, _localXp);
    }
    
    private void AddToPlayerXpProgression(Kaiju playerKaiju, int xpToAdd)
    {
        playerKaiju.levelProgression += xpToAdd;
        _statusHandler.DisplayXPGain(xpToAdd);
        if (playerKaiju.levelProgression < playerKaiju.nextXp) return;
        var remainder = playerKaiju.levelProgression - playerKaiju.nextXp;
        playerKaiju.Level++;
        playerKaiju.levelProgression = remainder;
        nextXp = KaijuStats.BaseXP * Level;
    }
}
