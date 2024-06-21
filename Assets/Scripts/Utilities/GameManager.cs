using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerInputDisabled { get; private set; }
    PlayerController _player;
    EnemyKaijuSpawner _enemyKaiju;

    public GameState GameState;

    string _battleScene = "BattleScene";
    string _overWorldScene = "Route_01";

    private void Start()
    {
        GameState = GameState.ROAM;
        _player = FindFirstObjectByType<PlayerController>();

        _player.OnEncounter += EnterBattle;
        _player.OnEnterTrainerView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponent<Trainer>();
            if (trainer != null) { StartCoroutine(trainer.TriggerTrainerBattle(_player)); }
        };
    }

    void Update()
    {
        if (GameState == GameState.ROAM)
        {
            _player.HandleUpdate();
        }
    }

    void EnterBattle()
    {
        GameState = GameState.BATTLE;
        SceneManager.LoadScene(_battleScene);
        _enemyKaiju = FindFirstObjectByType<EnemyKaijuSpawner>();
    }

    void BattleOver()
    {
        GameState = GameState.ROAM;
        SceneManager.LoadScene(_overWorldScene);
        _enemyKaiju.OnBattleOver += BattleOver;
    }
}

public enum GameState 
{
    ROAM,
    BATTLE
}
