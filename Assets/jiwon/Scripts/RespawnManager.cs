using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager instance = null;

    public static RespawnManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private float respwanTime = 10.0f;

    [SerializeField] private Image respwanBackground;
    [SerializeField] private TMP_Text respwanTimer;

    PhotonView PV;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void PlayerRespawn(GameObject diePlayer)
    {
        StartCoroutine(StartPlayerRespawn(diePlayer));
    }

    IEnumerator StartPlayerRespawn(GameObject diePlayer)
    {
        PV.RPC("RPC_SetActive", RpcTarget.All, diePlayer, false);
        respwanBackground.gameObject.SetActive(true);

        while (respwanTime >= 0)
        {
            respwanTime -= Time.deltaTime;
            respwanTimer.text = ((int)respwanTime).ToString();
            yield return null;
        }

        respwanBackground.gameObject.SetActive(false);
        diePlayer.GetComponent<PlayerHealth>().curHealth = diePlayer.GetComponent<PlayerHealth>().maxHealth;
        respwanTime = 10.0f;
        PV.RPC("RPC_SetActive", RpcTarget.All, diePlayer, true);

        yield break;
    }

    [PunRPC]
    public void RPC_SetActive(GameObject diePlayer, bool isActive)
    {
        diePlayer.SetActive(isActive);
    }
}
