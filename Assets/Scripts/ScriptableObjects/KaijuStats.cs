using UnityEngine;

[CreateAssetMenu(fileName = "KaijuStats", menuName = "Scriptable Objects/KaijuStats")]
public class KaijuStats : ScriptableObject
{
    [Header("Kaiju Attributes")]
    [field: Space(10)]
    [field: SerializeField] public string KaijuName { get; private set; }
    [field: SerializeField] public Sprite BattleSprite { get; private set; }

    [field: Space(10)]
    [Header("Battle Attributes")]
    [field: Space(10)]
    [field: SerializeField] public Types Type { get; private set; }
    [field: SerializeField] public Types WeakType  { get; private set; }
    [field: SerializeField] public float Health  { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Defense { get; private set; }
    [field: SerializeField] public float SpAttack { get; private set; }
    [field: SerializeField] public float SpDefense { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    
}

public enum Types
{
    Fire, Air, Water, Earth, Dark
}