using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyKaijuSwap : MonoBehaviour
{
    private EnemyKaijuParty _enemyParty;
    private EnemyKaijuSpawner _spawnedEnemyKaiju;

    private RoundStatusHandler _statusHandler;
    
    private BattleMenuController _battleMenu;
    
    private int _kaijuOnTeam = 6;
    
    private int _randomKaiju;

    private void Start()
    {
        _enemyParty ??= FindFirstObjectByType<EnemyKaijuParty>();
        _spawnedEnemyKaiju ??= FindFirstObjectByType<EnemyKaijuSpawner>();
        _statusHandler ??= FindFirstObjectByType<RoundStatusHandler>();
        _battleMenu ??= FindFirstObjectByType<BattleMenuController>();

        for (var i = 0; i < _spawnedEnemyKaiju.SpawnedKaiju.Length; i++)
        {
            if(_spawnedEnemyKaiju.SpawnedKaiju[i]) continue;
            
            _kaijuOnTeam--;
        }
    }

    public void SwapInRandomKaiju()
    {
        var deadKaiju = 0;
        foreach (var kaiju in _spawnedEnemyKaiju.SpawnedKaiju)
        {
            if(!kaiju)continue;
            if (kaiju.IsDead)
            {
                deadKaiju++;
            }
        }

        if (deadKaiju >= _kaijuOnTeam)
        {
            Debug.Log("All kaiju are dead, therefore you win the fight");
            _statusHandler.DisplayBattleWon(_spawnedEnemyKaiju.SpawnedKaiju[_randomKaiju].KaijuStats.KaijuName);
            return;
        }
        
        _randomKaiju = Random.Range(0, _kaijuOnTeam);
        
        while (_spawnedEnemyKaiju.SpawnedKaiju[_randomKaiju].IsDead)
        {
            _randomKaiju = Random.Range(0, _kaijuOnTeam);
        }

        _spawnedEnemyKaiju.SpawnedKaiju[_randomKaiju].gameObject.SetActive(true);
        _battleMenu.RenewEnemyStatValues();
    }
}
