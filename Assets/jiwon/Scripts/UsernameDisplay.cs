using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TMP_Text text;

    private void Start()
    {
        // 본인 닉네임은 표시 안 함
        if(playerPV.IsMine)
        {
            gameObject.SetActive(false);
        }

        text.text = playerPV.Owner.NickName;
    }
}
