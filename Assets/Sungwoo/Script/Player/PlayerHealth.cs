using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;

    public bool isLive = true;

    private PhotonView pv;

    private void Start()
    {
        curHealth = maxHealth;
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            UIManager.Instance.HealthUpdate(curHealth / maxHealth);
        }
    }

    public void OnDamage(float damage)
    {
        if (GameManager.Instance.gameClearHandler.gameObject.activeSelf)
            return;

        curHealth -= damage;

        if (pv.IsMine)
        {
            UIManager.Instance.HealthUpdate(curHealth / maxHealth);

            if(curHealth <= 0)
            {
                RespawnManager.Instance.PlayerRespawn(this.gameObject);
            }
        }
    }

    public void PlayerSetActive(bool isActive)
    {
        pv.RPC(nameof(RPC_SetActive), RpcTarget.All, isActive);
    }

    [PunRPC]
    private void RPC_SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }
}