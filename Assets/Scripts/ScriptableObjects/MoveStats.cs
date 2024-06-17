using UnityEngine;

[CreateAssetMenu(fileName = "MoveStats", menuName = "Scriptable Objects/MoveStats")]
public class MoveStats : ScriptableObject
{
    [field: SerializeField] public string MoveName { get; private set; }
    [field: SerializeField] public Types MoveType { get; private set; }
    [field: SerializeField] public int PP { get; private set; }
    [field: SerializeField] public int Strength { get; private set; }
    [field: SerializeField] public int Accuracy { get; private set; }
    [field: SerializeField] public CombatType CombatType { get; private set; }
}

public enum CombatType
{
    Physical, Special, StatusEffect
}
