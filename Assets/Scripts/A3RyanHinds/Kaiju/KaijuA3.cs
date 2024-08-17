using System.Collections.Generic;
using UnityEngine;

public class KaijuA3 : MonoBehaviour
{
    public KaijuBase KaijuBase { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }

    public int HP { get; set; }

    [field: SerializeField] public  List<MoveStats> LearnedMoves;

    public KaijuA3(KaijuBase kaijuBase, int level)
    {
        this.KaijuBase = kaijuBase;
        this.Level = level;
        this.Name = kaijuBase.KaijuName;
        this.HP = KaijuBase.MaxHP;

        LearnedMoves = new List<MoveStats>();
    }

    public int Attack
    {
        get
        {
            return Mathf.FloorToInt((KaijuBase.Attack * Level) / 100f + 5);
        }
    }

    public int Defence
    {
        get
        {
            return Mathf.FloorToInt((KaijuBase.Defence * Level) / 100f + 5);
        }
    }

    public int SpAttack
    {
        get
        {
            return Mathf.FloorToInt((KaijuBase.SpAttack * Level) / 100f + 5);
        }
    }

    public int SpDefence
    {
        get
        {
            return Mathf.FloorToInt((KaijuBase.SpDefence * Level) / 100f + 5);
        }
    }

    public int Speed
    {
        get
        {
            return Mathf.FloorToInt((KaijuBase.Speed * Level) / 100f + 5);
        }
    }

    public int MaxHP
    {
        get
        {
            return Mathf.FloorToInt((KaijuBase.MaxHP * Level) / 100f + 10);
        }
    }
}
