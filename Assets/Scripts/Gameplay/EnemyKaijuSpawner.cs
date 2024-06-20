using System;
using UnityEngine;

public class EnemyKaijuSpawner : MonoBehaviour
{
    private EnemyKaijuParty _kaijuParty;

    public Kaiju[] SpawnedKaiju { get; private set; } = new Kaiju[6];

    public event Action OnBattleOver;

    private void Start()
    {
        _kaijuParty = FindFirstObjectByType<EnemyKaijuParty>();

        for (var i = 0; i < _kaijuParty.KaijuInParty.Count; i++)
        {
            if (!_kaijuParty.KaijuInParty[i])
            {
                SpawnedKaiju[i] = null;
                return;
            }
            
            var kaijuSpawned = Instantiate(_kaijuParty.KaijuInParty[i]);
            SpawnedKaiju[i] = kaijuSpawned;
            if (i > 0)
            {
                SpawnedKaiju[i].gameObject.SetActive(false);
            }
        }
    }

    public void CheckAllKaijuAlive()
    {
        bool allInactive = true;
        foreach (var kaiju in SpawnedKaiju)
        {
            if (kaiju != null && kaiju.gameObject.activeInHierarchy)
            {
                allInactive =  false;
                break;
            }
        }

        if (allInactive)
        {
            OnBattleOver?.Invoke();
        }
    }
}