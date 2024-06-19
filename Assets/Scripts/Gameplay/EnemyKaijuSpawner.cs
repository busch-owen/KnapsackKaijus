using UnityEngine;

public class EnemyKaijuSpawner : MonoBehaviour
{
    private EnemyKaijuParty _kaijuParty;

    public Kaiju[] SpawnedKaiju { get; private set; } = new Kaiju[6];

    private void Awake()
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
}