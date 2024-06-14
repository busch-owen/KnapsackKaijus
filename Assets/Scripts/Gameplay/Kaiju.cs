using UnityEngine;
using UnityEngine.Events;

public class Kaiju : MonoBehaviour
{
    #region Local stats

    [SerializeField] private KaijuStats kaijuStats;

    private Types _localType;
    private Types _localWeakType;
    private float _localHealth;
    private float _currentHealth;
    private float _localAttack;
    private float _localDefense;
    private float _localSpAttack;
    private float _localSpDefense;
    private float _localSpeed;

    [field: SerializeField] public MoveStats[] LearnedMoves { get; private set; } = new MoveStats[4];

    [SerializeField] private int level;
    [SerializeField] private float statLevelIncrement;
    [SerializeField] private float statLevelMultiplier;
    [SerializeField] private float levelProgression;

    #endregion

    private BattleMenuController _battleMenuController;

    private void Awake()
    {
        statLevelMultiplier = level / statLevelIncrement;
        
        _localType = kaijuStats.Type;
        _localWeakType = kaijuStats.WeakType;
        _localHealth = kaijuStats.Health * statLevelMultiplier;
        _currentHealth = _localHealth;
        _localAttack = kaijuStats.Attack * statLevelMultiplier;
        _localDefense = kaijuStats.Defense * statLevelMultiplier;
        _localSpAttack = kaijuStats.SpAttack * statLevelMultiplier;
        _localSpDefense = kaijuStats.SpDefense * statLevelMultiplier;
        _localSpeed = kaijuStats.Speed * statLevelMultiplier;

        _battleMenuController = FindFirstObjectByType<BattleMenuController>();
    }

    private void TakeDamage(float damageToDeal, MoveStats movePerformed)
    {
        bool isPlayer;

        isPlayer = GetComponent<PlayerKaiju>();
        
        //Check for if the attack dealt is physical or special attack, then dampen damage accordingly
        //Additionally, check if the attacker's move type is a super effective type towards this Kaiju
        float effectiveMultiplier;
        
        if (movePerformed.MoveType == _localWeakType)
        {
            effectiveMultiplier = 2f;
        }
        else if (movePerformed.MoveType == _localType)
        {
            effectiveMultiplier = 0.5f;
        }
        else
        {
            effectiveMultiplier = 1f;
        }
        
        if (movePerformed.CombatType == CombatType.Physical)
        {
            _currentHealth -= damageToDeal / statLevelMultiplier * (_localDefense / 2.5f) * effectiveMultiplier;
            Debug.Log(_currentHealth);
        }
        else
        {
            _currentHealth -= damageToDeal / statLevelMultiplier * (_localSpDefense / 2.5f) * effectiveMultiplier;
            Debug.Log(_currentHealth);
        }
        
        if (isPlayer)
        {
            _battleMenuController.UpdatePlayerHealthBar(_currentHealth, _localHealth);
        }
        else
        {
            _battleMenuController.UpdateEnemyHealthBar(_currentHealth, _localHealth);
        }
    }

    public void Attack(Kaiju targetKaiju, int moveToPerformIndex)
    {
        
        
        Debug.Log(moveToPerformIndex);
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
}
