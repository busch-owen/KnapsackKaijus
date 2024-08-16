using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Kaiju", menuName = "A3/RyanHinds/SO/Kaiju/Create Kaiju", order = 0)]
public class KaijuBase : ScriptableObject
{
    #region KaijuFields
    [Header("Name of Kaiju")]
    [field: SerializeField] public string KaijuName;
    #endregion

    #region Kaiju Appearance
    [Header("Appearance")]
    [field: SerializeField] public Sprite FrontImage { get; private set; }
    [field: SerializeField] public Sprite BackImage { get; private set; }
    #endregion

    #region Stats
    [Header("Stats")]
    [field: SerializeField] public int MaxHP { get; private set; }
    [field: SerializeField] public int Attack { get; private set; }
    [field: SerializeField] public int Defence { get; private set; }
    [field: SerializeField] public int SpAttack { get; private set; }
    [field: SerializeField] public int SpDefence { get; private set; }
    [field: SerializeField] public int Speed { get; private set; }
    #endregion

    #region Type
    [Header("Type")]
    [field: SerializeField] public KaijuType KaijuType { get; private set; }
    #endregion
}

public enum KaijuType { Dark, Water, Air, Earth, Fire }
