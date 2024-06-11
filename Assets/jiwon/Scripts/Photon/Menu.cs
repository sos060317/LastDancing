using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    // 메뉴 열기
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    // 메뉴 닫기
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
