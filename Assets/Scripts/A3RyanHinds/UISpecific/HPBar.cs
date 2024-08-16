using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    public event Action<int> OnHealthChanged;

    public void SetHealth(int amount)
    {
        healthBar.maxValue = amount;
        healthBar.value = amount;
    }

    public void ChangeHealth(int amount) => OnHealthChanged?.Invoke(amount);
}
