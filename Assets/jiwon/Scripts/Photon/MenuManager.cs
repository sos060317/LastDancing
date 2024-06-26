using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 문자열 비교 후 해당 UI 표시, 다른 UI는 닫기
    /// </summary>
    /// <param name="menuName"></param>
    public void OpenMenu(string menuName)
    {
        for(int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    /// <summary>
    /// 메뉴 열기
    /// </summary>
    /// <param name="menu"></param>
    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    /// <summary>
    /// 메뉴 닫기
    /// </summary>
    /// <param name="menu"></param>
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
