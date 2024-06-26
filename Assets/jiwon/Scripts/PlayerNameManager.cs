using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;

    private void Start()
    {
        if (PlayerPrefs.HasKey("username")) // 기존 닉네임이 있으면 해당 닉네임 표시
        {
            usernameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else // 기본 닉네임이 없으면 랜덤 닉네임 부여 후 닉네임 설정
        {
            usernameInput.text = "Player " + Random.Range(0, 10000).ToString("0000");
            OnUsernameInputValueChanged();
        }
    }

    /// <summary>
    /// 닉네임 설정
    /// </summary>
    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInput.text;
        PlayerPrefs.SetString("username", usernameInput.text);
    }
}
