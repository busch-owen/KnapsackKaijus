using System;
using UnityEngine;
using BayatGames.SaveGameFree;

public class SaveDataManager : MonoBehaviour
{
    [SerializeField] private bool saveToggle = false;
    [SerializeField] private int testNumber; 
    private PlayerData playerParty;

    void Start()
    {
        testNumber = SaveGame.Load<int>("testNumber");
    }
    void Update()
    {
        if (saveToggle == true)
        {
            SaveGame.Save<int>("testNumber", testNumber);
        }
    }
}
