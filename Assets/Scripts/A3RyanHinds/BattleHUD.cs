using UnityEngine;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TMP_Text kaijuName;
    [SerializeField] TMP_Text kaijuLevel;


    public void SetKaijuData(KaijuA3 kaiju)
    {
        kaijuName.text = kaiju.KaijuBase.KaijuName;
        kaijuLevel.text = $"Lv {kaiju.Level}";
    }
}
