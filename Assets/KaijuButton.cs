using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class KaijuButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private Image healthBarFill;

    public void RefreshStats(string name, string level, float health)
    {
        nameText.text = name;
        levelText.text = $"LV: {level}";
        healthBarFill.fillAmount = health;
    }
}
