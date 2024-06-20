using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyKaijuSwap : MonoBehaviour
{
    private EnemyKaijuParty _enemyParty;
    private EnemyKaijuSpawner _spawnedEnemyKaiju;

    private BattleMenuController _battleMenu;
    
    private int _kaijuOnTeam = 6;

    private void Awake()
    {
        _enemyParty ??= FindFirstObjectByType<EnemyKaijuParty>();
        _spawnedEnemyKaiju ??= FindFirstObjectByType<EnemyKaijuSpawner>();
        _battleMenu ??= FindFirstObjectByType<BattleMenuController>();
    }

    public void SwapInRandomKaiju()
    {
        foreach (var kaiju in _spawnedEnemyKaiju.SpawnedKaiju)
        {
            if (!kaiju)
            {
                _kaijuOnTeam--;
                Debug.Log(_kaijuOnTeam);
            }
        }

        if (_kaijuOnTeam == 0)
        {
            //End the battle?
            return;
        }
        var randomKaiju = Random.Range(0, _kaijuOnTeam);
        
        while (_spawnedEnemyKaiju.SpawnedKaiju[randomKaiju].IsDead)
        {
            randomKaiju = Random.Range(0, _spawnedEnemyKaiju.SpawnedKaiju.Length);
        }

        _spawnedEnemyKaiju.SpawnedKaiju[randomKaiju].gameObject.SetActive(true);
        _battleMenu.RenewEnemyStatValues();
    }
}
