using UnityEngine;

[CreateAssetMenu(fileName = "KaijuStats", menuName = "Scriptable Objects/KaijuStats")]
public class KaijuStats : ScriptableObject
{
    [Header("Battle Attributes"), Space(10)]
    [SerializeField] private Types type;
    [SerializeField] private Types weakType;
    [SerializeField] private float health;
    [SerializeField] private float attack;
    [SerializeField] private float defense;
    [SerializeField] private float spAttack;
    [SerializeField] private float spDefense;
    [SerializeField] private float speed;
    [SerializeField] private Sprite battleSprite;
}

enum Types
{
    Fire, Air, Water, Earth, Dark
}