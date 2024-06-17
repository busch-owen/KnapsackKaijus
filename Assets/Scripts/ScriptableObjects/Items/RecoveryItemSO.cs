using UnityEngine;

[CreateAssetMenu(fileName = "ItemBaseSO", menuName = "Scriptable Objects/ Create Recovery Item")]
public class RecoveryItemSO : ItemBaseSO
{
    [Header("Recovery Stats")]
    [SerializeField] private int recoveryAmount;
}
