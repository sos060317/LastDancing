using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float curHealth;

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
}