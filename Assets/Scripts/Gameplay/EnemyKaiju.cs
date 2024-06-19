using UnityEngine;

public class EnemyKaiju : Kaiju
{

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
        AddToPlayerXpProgression(_targetKaiju, _localXp);
        
        base.Die();
        
        //Check if there is another kaiju in the enemy's party, if so, swap it, if not, end the battle
        var enemyParty = FindFirstObjectByType<EnemyKaijuParty>();
        var spawnedEnemyKaiju = FindFirstObjectByType<EnemyKaijuSpawner>();
        
        foreach (var kaiju in spawnedEnemyKaiju.SpawnedKaiju)
        {
            if (!kaiju) continue;
            if (kaiju.IsDead) continue;
            
            if (enemyParty.KaijuInParty.Count <= 1) continue;
                
            var randomKaijuToSendOut = Random.Range(0, enemyParty.KaijuInParty.Count);
            while (spawnedEnemyKaiju.SpawnedKaiju[randomKaijuToSendOut].IsDead)
            {
                randomKaijuToSendOut = Random.Range(0, enemyParty.KaijuInParty.Count);
            }

            if (spawnedEnemyKaiju.SpawnedKaiju[randomKaijuToSendOut].IsDead) continue;
                
            spawnedEnemyKaiju.SpawnedKaiju[randomKaijuToSendOut].gameObject.SetActive(true);
        }
        _battleMenuController.RenewEnemyStatValues();
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
