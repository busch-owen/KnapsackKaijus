using System;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    #region Events
    public static event Action OnMenuSelected, OnMenuCanceled;
    public static event Action<Vector2> MenuNav;
    #endregion

    public void HandleMenuNav(Vector2 navDir) => MenuNav?.Invoke(navDir);
    public void HandleAButton() => OnMenuSelected?.Invoke();
    public void HandleBButton() => OnMenuCanceled?.Invoke();
}
