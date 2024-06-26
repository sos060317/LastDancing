using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    /// <summary>
    /// 메뉴 열기
    /// </summary>
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 메뉴 닫기
    /// </summary>
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
