using UnityEngine;
using UnityEngine.Events;

public class Kaiju : MonoBehaviour
{
    #region Local stats

    [field: SerializeField] public KaijuStats KaijuStats { get; private set; }

    private Types _localType;
    private Types _localWeakType;
    private float _localHealth;
    private float _currentHealth;
    private float _localAttack;
    private float _localDefense;
    private float _localSpAttack;
    private float _localSpDefense;
    public float LocalSpeed { get; private set; }

    [field: SerializeField] public MoveStats[] LearnedMoves { get; private set; } = new MoveStats[4];

    [field: SerializeField] public int Level { get; private set; }
    [SerializeField] private float statLevelIncrement;
    [SerializeField] private float statLevelMultiplier;
    [SerializeField] private float levelProgression;

    #endregion

    private BattleMenuController _battleMenuController;
    private RoundStatusHandler _statusHandler;

    private void Awake()
    {
        statLevelMultiplier = Level / statLevelIncrement;
        
        _localType = KaijuStats.Type;
        _localWeakType = KaijuStats.WeakType;
        _localHealth = KaijuStats.Health * statLevelMultiplier;
        _currentHealth = _localHealth;
        _localAttack = KaijuStats.Attack * statLevelMultiplier;
        _localDefense = KaijuStats.Defense * statLevelMultiplier;
        _localSpAttack = KaijuStats.SpAttack * statLevelMultiplier;
        _localSpDefense = KaijuStats.SpDefense * statLevelMultiplier;
        LocalSpeed = KaijuStats.Speed * statLevelMultiplier;

        _battleMenuController = FindFirstObjectByType<BattleMenuController>();
        _statusHandler = FindFirstObjectByType<RoundStatusHandler>();
    }

    private void TakeDamage(float damageToDeal, MoveStats movePerformed)
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
            _currentHealth -= damageToDeal / statLevelMultiplier * (_localDefense / 2.5f) * effectiveMultiplier;
        }
        else
        {
            _currentHealth -= damageToDeal / statLevelMultiplier * (_localSpDefense / 2.5f) * effectiveMultiplier;
        }
        
        bool isPlayer = GetComponent<PlayerKaiju>();
        if (isPlayer)
        {
            _battleMenuController.UpdatePlayerHealthBar(_currentHealth, _localHealth);
        }
        else
        {
            _battleMenuController.UpdateEnemyHealthBar(_currentHealth, _localHealth);
        }

        if (_currentHealth <= 0)
        {
            _statusHandler.AddToDetails($"{KaijuStats.KaijuName} has brutally died");
            //Die();
        }
    }

    public void Attack(Kaiju targetKaiju, int moveToPerformIndex)
    {
        _statusHandler.AddToDetails($"{KaijuStats.KaijuName} used {LearnedMoves[moveToPerformIndex].MoveName}!");
        switch (LearnedMoves[moveToPerformIndex].CombatType)
        {
            case CombatType.Special:
                targetKaiju.TakeDamage(_localSpAttack, LearnedMoves[moveToPerformIndex]);
                break;
            case CombatType.StatusEffect:
                //Do status effect stuff here
                break;
            case CombatType.Physical:
            default:
                targetKaiju.TakeDamage(_localAttack, LearnedMoves[moveToPerformIndex]);
                break;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
