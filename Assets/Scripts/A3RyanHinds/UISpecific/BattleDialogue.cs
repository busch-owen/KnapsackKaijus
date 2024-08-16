using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text battleDialogue;
    [SerializeField] int lettersPersecond;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDeets;

    [SerializeField] List<TMP_Text> actiontexts;
    [SerializeField] List<TMP_Text> moveTexts;

    [SerializeField] TMP_Text attacksRemainingText;
    [SerializeField] TMP_Text typeText;

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

    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDeets.SetActive(enabled);
    }
}
