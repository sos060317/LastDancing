using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text text;
    private Player player;

    /// <summary>
    /// 플레이어 초기화
    /// </summary>
    /// <param name="_player"></param>
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    /// <summary>
    /// 플레이어가 방을 떠나면 해당 플레이어 파괴
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 플레이어가 방을 떠나면 해당 플레이어 파괴
    /// </summary>
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
