using System;
using UnityEngine;

public class MenuListener : MonoBehaviour
{
    [SerializeField] private GameObject _overWorldMenu;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        UIManager.Instance.OnMenuOpen += HandleMenu;
    }

    private void Start() { _overWorldMenu.SetActive(false); }

    void HandleMenu(bool menuState)
    {
        if (menuState)
        {
            _overWorldMenu.SetActive(true);
        }
        else
        {
            _overWorldMenu.SetActive(false);
        }
    }
}
