using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Kaiju : MonoBehaviour
{
    #region Local stats
    [field: SerializeField] public KaijuStats KaijuStats { get; private set; }
    
    private Types _localType;
    private Types _localWeakType;
    public float LocalHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    private float _localAttack;
    private float _localDefense;
    private float _localSpAttack;
    private float _localSpDefense;
    public float LocalSpeed { get; private set; }

    [field: SerializeField] public MoveStats[] LearnedMoves { get; private set; } = new MoveStats[4];
    public int[] MovePP { get; private set; }  = new int[4];

    [field: SerializeField] public int Level { get; internal set; }
    [SerializeField] internal int levelProgression;
    protected int _localXp;
    [SerializeField] internal int nextXp;
    [SerializeField] private float statLevelIncrement;
    [SerializeField] private float statLevelMultiplier;

    public bool IsDead { get; private set; }
    
    #endregion
    
    #region Handlers and Events
    protected BattleMenuController _battleMenuController;
    protected RoundStatusHandler _statusHandler;
    private KaijuSpawner _spawner;

    private UnityEvent _kaijuHasDied;

    protected Kaiju _targetKaiju;
    #endregion
    
    #region Misc Variables

    private WaitForFixedUpdate _waitForFixedUpdate;
    
    #endregion

    private void Awake()
    {
        statLevelMultiplier = Level / statLevelIncrement;
        
        _localType = KaijuStats.Type;
        _localWeakType = KaijuStats.WeakType;
        LocalHealth = KaijuStats.Health * statLevelMultiplier;
        CurrentHealth = LocalHealth;
        _localAttack = KaijuStats.Attack * statLevelMultiplier;
        _localDefense = KaijuStats.Defense * statLevelMultiplier;
        _localSpAttack = KaijuStats.SpAttack * statLevelMultiplier;
        _localSpDefense = KaijuStats.SpDefense * statLevelMultiplier;
        LocalSpeed = KaijuStats.Speed * statLevelMultiplier;
        _localXp = (int)(KaijuStats.BaseXP * statLevelMultiplier);
        nextXp = KaijuStats.BaseXP * Level;

        _battleMenuController = FindFirstObjectByType<BattleMenuController>();
        _statusHandler = FindFirstObjectByType<RoundStatusHandler>();
        _spawner = FindFirstObjectByType<KaijuSpawner>();

        _kaijuHasDied ??= new UnityEvent();
    }

    private void Start()
    {
        for(var i = 0; i < LearnedMoves.Length; i++)
        {
            if (LearnedMoves[i] == null) return;
            
            MovePP[i] = LearnedMoves[i].PP;
        }
    }

    protected virtual void TakeDamage(float damageToDeal, MoveStats movePerformed)
    {
        //Check for if the attack dealt is physical or special attack, then dampen damage accordingly
        //Additionally, check if the attacker's move type is a super effective type towards this Kaiju
        float effectiveMultiplier;
        
        if (movePerformed.MoveType == _localWeakType)
        {
            effectiveMultiplier = 2f;
            _statusHandler.AddToDetails($"It was super effective!");
        }
        else if (movePerformed.MoveType == _localType)
        {
            effectiveMultiplier = 0.5f;
            _statusHandler.AddToDetails($"It wasn't very effective...");
        }
        else
        {
            effectiveMultiplier = 1f;
        }
        
        if (movePerformed.CombatType == CombatType.Physical)
        {
            //_currentHealth -= damageToDeal / statLevelMultiplier * (_localDefense / 2.5f) * effectiveMultiplier;
            StartCoroutine(LerpHealthValue(damageToDeal / statLevelMultiplier * (_localDefense / 2.5f) * effectiveMultiplier, effectiveMultiplier * 2));
        }
        else
        {
            //_currentHealth -= damageToDeal / statLevelMultiplier * (_localSpDefense / 2.5f) * effectiveMultiplier;
            StartCoroutine(LerpHealthValue(damageToDeal / statLevelMultiplier * (_localSpDefense / 2.5f) * effectiveMultiplier, effectiveMultiplier * 2));
        }
    }

    public virtual void Attack(Kaiju targetKaiju, int moveToPerformIndex)
    {
        if (IsDead) return;
        
        _targetKaiju = targetKaiju;
        _statusHandler.AddToDetails($"{KaijuStats.KaijuName} used {LearnedMoves[moveToPerformIndex].MoveName}!");

        if (MovePP[moveToPerformIndex] <= 0)
        {
            _statusHandler.AddToDetails($"But it failed...");
            return;
        }
        var moveWillHit = Random.Range(0, 100);
        if (moveWillHit > LearnedMoves[moveToPerformIndex].Accuracy)
        {
            _statusHandler.AddToDetails($"But it missed...");
            return;
        }

        var movePower = (100 - LearnedMoves[moveToPerformIndex].Strength) / 100f;
        
        MovePP[moveToPerformIndex]--;
        
        switch (LearnedMoves[moveToPerformIndex].CombatType)
        {
            case CombatType.Special:
                targetKaiju.TakeDamage(_localSpAttack / movePower, LearnedMoves[moveToPerformIndex]);
                break;
            case CombatType.StatusEffect:
                //Do status effect stuff here
                break;
            case CombatType.Physical:
            default:
                targetKaiju.TakeDamage(_localAttack / movePower, LearnedMoves[moveToPerformIndex]);
                break;
        }
    }

    protected virtual void Die()
    {
        IsDead = true;
        gameObject.SetActive(false);
        if (!GetComponent<PlayerKaiju>())
        {
            //Check if there is another kaiju in the enemy's party, if so, swap it, if not, end the battle
            _statusHandler.DisplayBattleWon(KaijuStats.KaijuName);
        }
        else
        {
            //Allow player to swap a new Kaiju into battle so long as there is another Kaiju to swap in
        }
    }

    private IEnumerator LerpHealthValue(float valueDealt, float speed)
    {
        var newValue = CurrentHealth - valueDealt;
        newValue = Mathf.Clamp(newValue, -0.1f, CurrentHealth);

        while (!Mathf.Approximately(CurrentHealth, newValue))
        {
            CurrentHealth = Mathf.MoveTowards(CurrentHealth, newValue, speed * Time.fixedDeltaTime);
            bool isPlayer = GetComponent<PlayerKaiju>();
            
            if (isPlayer)
            {
                _battleMenuController.UpdatePlayerHealthBar(CurrentHealth, LocalHealth);
            }
            else
            {
                _battleMenuController.UpdateEnemyHealthBar(CurrentHealth, LocalHealth);
            }
            
            if (CurrentHealth < 0)
            {
                _statusHandler.AddToDetails($"{KaijuStats.KaijuName} has brutally died");
                Die();
                yield break;
            }
            yield return _waitForFixedUpdate;
        }
    }
}
