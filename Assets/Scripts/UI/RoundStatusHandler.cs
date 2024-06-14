using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundStatusHandler : MonoBehaviour
{
    private List<string> _detailsToDisplay = new List<string>();

    [SerializeField] private GameObject displayWindow;
    [SerializeField] private TMP_Text displayText; 

    [SerializeField] private float timeBetweenDetails;

    private WaitForSeconds _waitForDuration;

    private void Start()
    {
        _waitForDuration ??= new WaitForSeconds(timeBetweenDetails);
        displayWindow.SetActive(false);
    }

    public void AddToDetails(string detailToAdd)
    {
        _detailsToDisplay.Add(detailToAdd);
    }

    public IEnumerator DisplayDetails()
    {
        displayWindow.SetActive(true);
        foreach (var detail in _detailsToDisplay)
        {
            displayText.text = detail;
            yield return _waitForDuration;
        }
        displayWindow.SetActive(false);
    }
}
