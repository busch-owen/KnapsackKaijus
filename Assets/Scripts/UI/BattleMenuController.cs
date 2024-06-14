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

    private UnityEvent _cancelPressed;

    private void Awake()
    {
        _eventSystem = FindFirstObjectByType<EventSystem>();
        _inputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        
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
}
