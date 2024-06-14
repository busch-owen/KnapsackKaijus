using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class BattleMenuController : MonoBehaviour
{
    private EventSystem _eventSystem;
    private InputSystemUIInputModule _inputModule;
    [SerializeField] private GameObject attackMenu;
    [SerializeField] private GameObject interactionMenu;
    private GameObject _currentMenu;

    private Kaiju _playerKaiju;
    private EnemyKaiju _enemyKaiju;
    private UnityEvent _cancelPressed;

    private Button[] _attackButtons;

    [field: SerializeField] public Image EnemyHealthBar { get; private set; }
    [field: SerializeField] public Image PlayerHealthBar { get; private set; }

    private void Awake()
    {
        _eventSystem = FindFirstObjectByType<EventSystem>();
        _inputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        _attackButtons = attackMenu.GetComponentsInChildren<Button>();
        _playerKaiju = FindFirstObjectByType<PlayerKaiju>();
        _enemyKaiju = FindFirstObjectByType<EnemyKaiju>();

        for (int i = 0; i < _attackButtons.Length; i++)
        {
            if (_playerKaiju.LearnedMoves[i] != null)
            {
                Debug.Log(i);
                var i1 = i;
                _attackButtons[i].onClick.AddListener(delegate { _playerKaiju.Attack(_enemyKaiju, i1);});
            }
            else
            {
                _attackButtons[i].gameObject.SetActive(false);
            }
        }
        
        attackMenu.SetActive(false);
        
        _cancelPressed ??= new UnityEvent();
        
        AssignEventListeners();
    }

    private void Update()
    {
        CancelPressed();
    }

    public void OpenSpecificMenu(GameObject menuToOpen)
    {
        menuToOpen.SetActive(true);
        
        GameObject buttonToTarget = menuToOpen.GetComponentInChildren<Button>().gameObject;
        if (buttonToTarget)
        {
            _eventSystem.SetSelectedGameObject(menuToOpen.GetComponentInChildren<Button>().gameObject);
        }
        
        _currentMenu = menuToOpen;
        interactionMenu.SetActive(false);
    }

    private void ReturnToMainMenu()
    {
        _currentMenu.SetActive(false);
        interactionMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(interactionMenu.GetComponentInChildren<Button>().gameObject);
    }

    //Detects if the cancel button was pressed which will tell the UI manager to return to previous menu
    private void CancelPressed()
    {
        if (_inputModule.cancel.action.ReadValue<float>() > 0)
        {
            _cancelPressed.Invoke();
        }
    }

    private void AssignEventListeners()
    {
        _cancelPressed.AddListener(ReturnToMainMenu);
    }

    public void UpdateEnemyHealthBar(float valueToUpdate, float valueToMultBy)
    {
        EnemyHealthBar.fillAmount = valueToUpdate / valueToMultBy;
        Debug.LogFormat($"The new current health is {valueToUpdate}. The value to divide by is {valueToMultBy}. Combined, the value getting posted on the health bar is {valueToUpdate / valueToMultBy}");
    }
    public void UpdatePlayerHealthBar(float valueToUpdate, float valueToMultBy)
    {
        PlayerHealthBar.fillAmount = valueToUpdate / valueToMultBy;
    }
}
