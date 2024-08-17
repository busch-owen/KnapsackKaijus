using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text battleDialogue;
    [SerializeField] int lettersPersecond;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject fightRunOption;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDeets;

    [field: SerializeField] public List<TMP_Text> actionTexts;
    [SerializeField] List<TMP_Text> moveTexts;

    [SerializeField] TMP_Text attacksRemainingText;
    [SerializeField] TMP_Text typeText;

    Color _highlightedColor = new Color(0.188f, 0.384f, 0.188f, 0);
    Color _originalColor = new Color(0.188f, 0.384f, 0.188f, 1f);
    Coroutine colorShiftCoroutine;

    public void SetBattleText(string battleText)
    {
        battleDialogue.text = battleText;
    }

    public IEnumerator TypeText(string dialogue)
    {
        battleDialogue.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            battleDialogue.text += letter;
            yield return new WaitForSeconds(1f/lettersPersecond);
        }
    }

    public void EnableDialogueText(bool enabled)
    {
        battleDialogue.enabled = enabled;
    }

    public void EnablActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableFightMenu(bool enabled)
    {
        fightRunOption.SetActive(enabled);
    }

    public void EnableMoveSelector(bool enabled)
    {
        fightRunOption.SetActive(false);
        moveSelector.SetActive(enabled);
        moveDeets.SetActive(enabled);
    }

    public void UpdateActionSelector(int indexSelection)
    {
        if (colorShiftCoroutine != null)
        {
            StopCoroutine(colorShiftCoroutine);
        }
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i == indexSelection)
            {
                colorShiftCoroutine = StartCoroutine(ColorShift(i));
            }
        }
    }
    public void SetMoveName(List<MoveStats> moves)
    {
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].MoveName;
            }
            else
            {
                moveTexts[i].text = "---";
            }
        }
    }

    IEnumerator ColorShift(int indexSelection)
    {
        float elapsedTime = 0f;
        float duration = 1f / 3f;

        TMP_Text selectedText = actionTexts[indexSelection];

        Color startingColor = actionTexts[indexSelection].color;
        Color endColor = startingColor == _originalColor ? _highlightedColor : _originalColor;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            selectedText.color = Color.Lerp(startingColor, endColor, elapsedTime / duration);
            
            yield return null;
        }

        selectedText.color = endColor;
        colorShiftCoroutine = null;
    }
}
