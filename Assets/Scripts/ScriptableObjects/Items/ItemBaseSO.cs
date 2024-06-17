using System;
using System.Collections.Generic;
using NUnit.Framework.Internal.Execution;
using UnityEngine;

public class ItemBaseSO : ScriptableObject
{
    [Header("Item Basics")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private int itemCost;
}

enum ItemType
{
    Recovery, BattleItem, LevelingItem
}