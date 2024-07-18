using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            CreateController();
        }
    }

    /// <summary>
    /// 게임 시작 시, 플레이어 생성
    /// </summary>
    private void CreateController()
    {
        GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero, Quaternion.identity);
        PV.RPC(nameof(RPC_AddPlayer), RpcTarget.All, player.transform.GetChild(0).gameObject.activeSelf);
    }

    [PunRPC]
    private void RPC_AddPlayer(bool player)
    {
        GameManager.Instance.players.Add(player);
    }
}
