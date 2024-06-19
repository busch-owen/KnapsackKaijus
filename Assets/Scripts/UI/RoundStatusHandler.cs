using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundStatusHandler : MonoBehaviour
{
    public List<string> DetailsToDisplay = new();

    [SerializeField] private GameObject displayWindow;
    [SerializeField] private TMP_Text displayText; 

    [SerializeField] private float timeBetweenDetails;

    private TurnHandler _turnHandler;
    
    private UnityEvent _firstDetailsFinished;

    private WaitForSeconds _waitForDuration;

    private void Start()
    {
        _waitForDuration ??= new WaitForSeconds(timeBetweenDetails);
        _turnHandler = FindFirstObjectByType<TurnHandler>();
        displayWindow.SetActive(false);

        _firstDetailsFinished ??= new UnityEvent();
        _firstDetailsFinished.AddListener(_turnHandler.SecondTurn);
    }

    public void AddToDetails(string detailToAdd)
    {
        DetailsToDisplay.Add(detailToAdd);
    }

    public IEnumerator DisplayDetails()
    {
        displayWindow.SetActive(true);
        if (DetailsToDisplay.Count <= 0) yield break;
        for(var i = 0; i < DetailsToDisplay.Count; i++)
        {
            displayText.text = DetailsToDisplay[i];
            yield return _waitForDuration;
        }
        InvokeFirstTurnFinished();
    }

    public void DisplayBattleWon(string nameOfOpponent)
    {
        StopCoroutine(DisplayDetails());
        AddToDetails($"You have defeated {nameOfOpponent}!");
        StartCoroutine(DisplayDetails());
    }

    public void DisplayXpGain(int xpGained)
    {
        StopCoroutine(DisplayDetails());
        AddToDetails($"You gained {xpGained}XP!");
        StartCoroutine(DisplayDetails());
    }

    public void DisplayLevelUpInformation(Kaiju targetKaiju, int newLevel)
    {
        StopCoroutine(DisplayDetails());
        AddToDetails($"{targetKaiju.KaijuStats.KaijuName} leveled up to level {newLevel}!");
        StartCoroutine(DisplayDetails());
    }

    public void InvokeFirstTurnFinished()
    {
        if (!_turnHandler.AttackerTwoTurn)
            _firstDetailsFinished.Invoke();
        ClearDetails();
    }

    public void ClearDetails()
    {
        displayWindow.SetActive(false);
        DetailsToDisplay.Clear();
    }
}
