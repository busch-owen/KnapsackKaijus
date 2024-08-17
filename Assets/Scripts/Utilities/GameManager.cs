using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool IsPlayerInputDisabled { get; private set; }
    PlayerController _player;
    EnemyKaijuSpawner _enemyKaiju;

    InputManager _inputManager;

    [SerializeField] GameObject _battleCamera;
    [SerializeField] GameObject _mainCamera;

    public GameState GameState;

    private void Awake()
    {
        GameState = GameState.BATTLE;
        _player = FindFirstObjectByType<PlayerController>();
        _inputManager = FindFirstObjectByType<InputManager>();
        _battleCamera = GameObject.FindGameObjectWithTag("BattleCamera");
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        _player.OnEncounter += EnterBattle;
        _player.OnEnterTrainerView += (Collider2D trainerCollider) =>
        {
            var trainer = trainerCollider.GetComponent<Trainer>();
            if (trainer != null) { StartCoroutine(trainer.TriggerTrainerBattle(_player)); }
        };
    }

    void Start()
    {
        // this is temporary
        _inputManager.EnableInput(GameInputType.Battle);
        _inputManager.DisableInput(GameInputType.Player);
    }

    void Update()
    {
        if (GameState == GameState.BATTLE)
        {
            _inputManager.EnableInput(GameInputType.Battle);
            _inputManager.DisableInput(GameInputType.Player);
        }
        else if (GameState == GameState.ROAM)
        {
            _inputManager.EnableInput(GameInputType.Player);
            _inputManager.DisableInput(GameInputType.Battle);
        }
    }

    void EnterBattle()
    {
        GameState = GameState.BATTLE;
        _battleCamera.SetActive(true);
        _mainCamera.SetActive(false);
    }

    void BattleOver()
    {
        GameState = GameState.ROAM;
        _battleCamera.SetActive(false);
        _mainCamera.SetActive(true);
        _enemyKaiju.OnBattleOver += BattleOver;
    }

    void OnDisable()
    {
        _player.OnEncounter -= EnterBattle;
    }
}

public enum GameState 
{
    ROAM,
    BATTLE
}
