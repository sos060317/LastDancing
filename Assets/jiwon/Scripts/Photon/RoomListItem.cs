using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public RoomInfo info;

    /// <summary>
    /// 방 초기화
    /// </summary>
    /// <param name="_info"></param>
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    /// <summary>
    /// 방 입장
    /// </summary>
    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
    }
}
