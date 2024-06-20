using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class RoundStatusHandler : MonoBehaviour
{
    public List<string> DetailsToDisplay = new();

    private EventSystem _eventSystem;
    private InputSystemUIInputModule _inputModule;
    [SerializeField] private GameObject displayWindow;
    [SerializeField] private TMP_Text displayText; 

    [SerializeField] private float timeBetweenDetails;

    private TurnHandler _turnHandler;
    
    private UnityEvent _firstDetailsFinished;

    private WaitForSeconds _waitForDuration;

    private void Start()
    {
        _eventSystem = FindFirstObjectByType<EventSystem>();
        _inputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        _waitForDuration ??= new WaitForSeconds(timeBetweenDetails);
        _turnHandler = FindFirstObjectByType<TurnHandler>();
        displayWindow.SetActive(false);

        _firstDetailsFinished ??= new UnityEvent();
        _firstDetailsFinished.AddListener(_turnHandler.SecondTurn);
    }

    private void FixedUpdate()
    {
        if (DetailsToDisplay.Count <= 0 && displayWindow.activeSelf)
        {
            displayWindow.SetActive(false);
            _inputModule.enabled = true;
        }
    }

    public void AddToDetails(string detailToAdd)
    {
        DetailsToDisplay.Add(detailToAdd);
    }

    public IEnumerator DisplayDetails()
    {
        displayWindow.SetActive(true);
        _inputModule.enabled = false;
        if (DetailsToDisplay.Count <= 0) yield break;
        for(var i = 0; i < DetailsToDisplay.Count; i++)
        {
            displayText.text = DetailsToDisplay[i];
            yield return _waitForDuration;
        }
        ClearDetails();
        InvokeFirstTurnFinished();
    }

    public void DisplayBattleWon(string nameOfOpponent)
    {
        StopCoroutine(DisplayDetails());
        AddToDetails($"You have defeated {nameOfOpponent}!");
        StartCoroutine(DisplayDetails());
        Invoke(nameof(LeaveBattle), 8f);
    }
    
    public void DisplayBattleLost()
    {
        StopCoroutine(DisplayDetails());
        AddToDetails($"All your Kaiju are dead, you go into shock and the enemy trainer flees...");
        StartCoroutine(DisplayDetails());
        Invoke(nameof(LeaveBattle), 8f);
    }

    public void LeaveBattle()
    {
        SceneManager.LoadScene("CharacterTestScene");
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
        if (_turnHandler.AttackerTwoTurn) return;
        
        _firstDetailsFinished.Invoke(); 
    }

    public void ClearDetails()
    {
        DetailsToDisplay.Clear();
    }
}
