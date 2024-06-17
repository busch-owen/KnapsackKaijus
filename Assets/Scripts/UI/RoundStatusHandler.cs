using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class RoundStatusHandler : MonoBehaviour
{
    public List<string> _detailsToDisplay = new List<string>();

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
        _detailsToDisplay.Add(detailToAdd);
    }

    public IEnumerator DisplayDetails()
    {
        displayWindow.SetActive(true);
        if (_detailsToDisplay.Count <= 0) yield break;
        for(var i = 0; i < _detailsToDisplay.Count; i++)
        {
            displayText.text = _detailsToDisplay[i];
            yield return _waitForDuration;
        }
        ClearDetails();
        displayText.text = "your shit broke jackass";
        displayWindow.SetActive(false);
        if (!_turnHandler.AttackerTwoTurn)
            _firstDetailsFinished.Invoke();
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

    public void ClearDetails()
    {
        _detailsToDisplay.Clear();
    }
        
}
