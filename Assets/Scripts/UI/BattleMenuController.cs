using System;
using TMPro;
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

    private TurnHandler _turnHandler;
    
    private Kaiju _playerKaiju;
    private EnemyKaiju _enemyKaiju;
    private UnityEvent _cancelPressed;

    private Button[] _attackButtons;

    private RoundStatusHandler _statusHandler;
    
    [field: SerializeField] public Image EnemyHealthBar { get; private set; }
    [field: SerializeField] public Image PlayerHealthBar { get; private set; }

    [SerializeField] private TMP_Text enemyName;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text enemyLvl;
    [SerializeField] private TMP_Text playerLvl;

    private void Awake()
    {
        _eventSystem = FindFirstObjectByType<EventSystem>();
        _turnHandler = FindFirstObjectByType<TurnHandler>();
        _statusHandler = FindFirstObjectByType<RoundStatusHandler>();
        _inputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        _attackButtons = attackMenu.GetComponentsInChildren<Button>();
        _playerKaiju = FindFirstObjectByType<PlayerKaiju>();
        _enemyKaiju = FindFirstObjectByType<EnemyKaiju>();
        
        RenewEnemyStatValues();
        RenewPlayerStatValues();

        for (int i = 0; i < _attackButtons.Length; i++)
        {
            if (_playerKaiju.LearnedMoves[i] != null)
            {
                var moveIndex = i;
                _attackButtons[i].onClick.AddListener(delegate {_turnHandler.DetermineFirstKaiju(_playerKaiju, _enemyKaiju, moveIndex); });
                _attackButtons[i].onClick.AddListener(delegate { StartCoroutine(_statusHandler.DisplayDetails()); });
                _attackButtons[i].onClick.AddListener(ReturnToMainMenu);
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


    public void RenewPlayerStatValues()
    {
        UpdateSpecificName(playerName, _playerKaiju.KaijuStats.KaijuName);
        UpdateLevelValue(playerLvl, _playerKaiju.Level);
    }
    private void RenewEnemyStatValues()
    {
        UpdateSpecificName(enemyName, _enemyKaiju.KaijuStats.KaijuName);
        UpdateLevelValue(enemyLvl, _enemyKaiju.Level);
    }
    
    public void UpdateSpecificName(TMP_Text nameText, string name)
    {
        nameText.text = name;
    }

    public void UpdateLevelValue(TMP_Text levelText, int value)
    {
        levelText.text = $"LV: {value}";
    }
    
    public void UpdateEnemyHealthBar(float valueToUpdate, float valueToMultBy)
    {
        EnemyHealthBar.fillAmount = valueToUpdate / valueToMultBy;
    }
    public void UpdatePlayerHealthBar(float valueToUpdate, float valueToMultBy)
    {
        PlayerHealthBar.fillAmount = valueToUpdate / valueToMultBy;
    }
}
