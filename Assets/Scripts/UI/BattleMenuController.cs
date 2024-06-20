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
    [SerializeField] private GameObject kaijuMenu;
    private GameObject _currentMenu;

    private TurnHandler _turnHandler;
    
    private Kaiju _playerKaiju;
    private EnemyKaiju _enemyKaiju;
    private UnityEvent _cancelPressed;

    private Button[] _attackButtons;
    private Button[] _kaijuButtons;

    private KaijuSpawner _spawner;

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
        _spawner = FindFirstObjectByType<KaijuSpawner>();
        _inputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        _attackButtons = attackMenu.GetComponentsInChildren<Button>();
        _kaijuButtons = kaijuMenu.GetComponentsInChildren<Button>();
        attackMenu.SetActive(false);
        kaijuMenu.SetActive(false);
        
        _cancelPressed ??= new UnityEvent();
        
        AssignEventListeners();
    }

    private void Start()
    {
        _playerKaiju = FindFirstObjectByType<PlayerKaiju>();
        _enemyKaiju = FindFirstObjectByType<EnemyKaiju>();
        RenewEnemyStatValues();
        RenewPlayerStatValues();

        RefreshKaijuButtons();
        RefreshAttackButtons();
    }

    private void Update()
    {
        CancelPressed();
    }

    public void OpenSpecificMenu(GameObject menuToOpen)
    {
        menuToOpen.SetActive(true);
        if (menuToOpen == kaijuMenu)
        {
            RefreshKaijuMenuStats();
        }

        var buttonToTarget = menuToOpen.GetComponentInChildren<Button>();
        if (buttonToTarget)
        {
            _eventSystem.SetSelectedGameObject(buttonToTarget.gameObject);
        }
        
        _currentMenu = menuToOpen;
        interactionMenu.SetActive(false);
    }

    private void RefreshAttackButtons()
    {
        for (var i = 0; i < _attackButtons.Length; i++)
        {
            if (_playerKaiju.LearnedMoves[i] != null)
            {
                var moveIndex = i;
                _attackButtons[i].onClick.RemoveAllListeners();
                _attackButtons[i].GetComponentInChildren<TMP_Text>().text = _playerKaiju.LearnedMoves[i].MoveName;
                _attackButtons[i].onClick.AddListener(delegate { _turnHandler.DetermineFirstKaiju(_playerKaiju, _enemyKaiju, moveIndex); RenewEnemyStatValues();});
                _attackButtons[i].onClick.AddListener(delegate { StartCoroutine(_statusHandler.DisplayDetails());});
                _attackButtons[i].onClick.AddListener(ReturnToMainMenu);
            }
            else
            {
                _attackButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshKaijuButtons()
    {
        for (var i = 0; i < _kaijuButtons.Length; i++)
        {
            if (_spawner.SpawnedKaiju[i] != null)
            {
                //Very hacky solution for now, if I have time I would like to add an inspect panel to see the stats of your Kaiju
                var kaijuIndex = i;
                
                _kaijuButtons[i].GetComponentInChildren<TMP_Text>().text = _spawner.SpawnedKaiju[i].KaijuStats.KaijuName;
                _kaijuButtons[i].onClick.AddListener(delegate{_statusHandler.AddToDetails($"Great job, {FindFirstObjectByType<PlayerKaiju>()?.KaijuStats.KaijuName}! Come back!"); });
                _kaijuButtons[i].onClick.AddListener(delegate{FindFirstObjectByType<PlayerKaiju>()?.gameObject.SetActive(false);});
                _kaijuButtons[i].onClick.AddListener(delegate{_spawner.SpawnedKaiju[kaijuIndex]?.gameObject.SetActive(true);});
                _kaijuButtons[i].onClick.AddListener(delegate{RenewPlayerStatValues(); RefreshAttackButtons();});
                _kaijuButtons[i].onClick.AddListener(delegate{_statusHandler.AddToDetails($"Go, {_spawner.SpawnedKaiju[kaijuIndex]?.KaijuStats.KaijuName}!");});
                _kaijuButtons[i].onClick.AddListener(delegate{ StartCoroutine(_statusHandler.DisplayDetails());});
                _kaijuButtons[i].onClick.AddListener(delegate{_turnHandler.ForfeitMove(_playerKaiju, _enemyKaiju);});
                
                _kaijuButtons[i].onClick.AddListener(ReturnToMainMenu);
            }
            else
            {
                _kaijuButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshKaijuMenuStats()
    {
        for (var i = 0; i < _kaijuButtons.Length; i++)
        {
            if (!_spawner.SpawnedKaiju[i]) continue;
            if (_spawner.SpawnedKaiju[i].IsDead)
            {
                _kaijuButtons[i].GetComponent<KaijuButton>().RefreshStats("*DEAD*", "~", 0);
                _kaijuButtons[i].onClick.RemoveAllListeners();
            }
            else
            {
                _kaijuButtons[i].GetComponent<KaijuButton>().RefreshStats(_spawner.SpawnedKaiju[i].KaijuStats.KaijuName, _spawner.SpawnedKaiju[i].Level.ToString(), 
                    _spawner.SpawnedKaiju[i].CurrentHealth/_spawner.SpawnedKaiju[i].LocalHealth);
            }
        }
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
        if(!_currentMenu) return;
        
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
        _playerKaiju = FindFirstObjectByType<PlayerKaiju>();
        if (!_playerKaiju) return;
        UpdateSpecificName(playerName, _playerKaiju.KaijuStats.KaijuName);
        UpdateLevelValue(playerLvl, _playerKaiju.Level);
    }
    public void RenewEnemyStatValues()
    {
        _enemyKaiju = FindFirstObjectByType<EnemyKaiju>();
        if (!_enemyKaiju) return;
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
