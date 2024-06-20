using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyKaijuSwap : MonoBehaviour
{
    private EnemyKaijuParty _enemyParty;
    private EnemyKaijuSpawner _spawnedEnemyKaiju;

    private BattleMenuController _battleMenu;
    
    private int _kaijuOnTeam = 6;

    private void Start()
    {
        _enemyParty ??= FindFirstObjectByType<EnemyKaijuParty>();
        _spawnedEnemyKaiju ??= FindFirstObjectByType<EnemyKaijuSpawner>();
        _battleMenu ??= FindFirstObjectByType<BattleMenuController>();

        for (var i = 0; i < _spawnedEnemyKaiju.SpawnedKaiju.Length; i++)
        {
            if(_spawnedEnemyKaiju.SpawnedKaiju[i]) continue;
            
            _kaijuOnTeam--;
        }
    }

    public void SwapInRandomKaiju(EnemyKaiju previousActiveKaiju)
    {
        var randomKaiju = Random.Range(0, _kaijuOnTeam);

        var deadKaiju = 0;
        foreach (var kaiju in _spawnedEnemyKaiju.SpawnedKaiju)
        {
            Debug.Log(kaiju);
            if(!kaiju)continue;
            if (kaiju.IsDead)
            {
                deadKaiju++;
            }
        }

        if (deadKaiju >= _kaijuOnTeam)
        {
            Debug.Log("All kaiju are dead, therefore you win the fight");
            return;
        }
        
        Debug.Log(randomKaiju);
        while (_spawnedEnemyKaiju.SpawnedKaiju[randomKaiju] == previousActiveKaiju || _spawnedEnemyKaiju.SpawnedKaiju[randomKaiju].IsDead)
        {
            randomKaiju = Random.Range(0, randomKaiju);
        }

        _spawnedEnemyKaiju.SpawnedKaiju[randomKaiju].gameObject.SetActive(true);
        _battleMenu.RenewEnemyStatValues();
    }
}
