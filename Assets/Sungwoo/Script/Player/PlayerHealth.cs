using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Animations.Rigging;

public class PlayerHealth : MonoBehaviour, IPunObservable
{
    public float maxHealth;
    public float curHealth;
    public PlayerWeapon weapon;

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
                weapon.isReload = false;
                weapon.curMagazine = weapon.maxMagazine;
                RespawnManager.Instance.PlayerRespawn(this.gameObject);
            }
        }
    }

    public void PlayerSetActive(bool isActive)
    {
        pv.RPC(nameof(RPC_SetActive), RpcTarget.All, isActive);
    }

    public void Heal(float healAmount)
    {
        curHealth = Mathf.Min(curHealth + healAmount, maxHealth);

        if (pv.IsMine)
        {
            UIManager.Instance.HealthUpdate(curHealth / maxHealth);
        }
    }

    [PunRPC]
    private void RPC_SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);

        for (int i = 0; i < GameManager.Instance.players.Count; i++)
        {
            if(isActive)
            {
                if (!GameManager.Instance.players[i])
                {
                    GameManager.Instance.players[i] = true;
                    break;
                }
            }
            else
            {
                if (GameManager.Instance.players[i])
                {
                    GameManager.Instance.players[i] = false;
                    break;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 자신이면 데이터 보내기
        if (stream.IsWriting)
        {
            stream.SendNext(this.gameObject.activeSelf);
        }
        // 자신이 아니면 데이터 받기
        else if (stream.IsReading)
        {
            bool isAction = (bool)stream.ReceiveNext();
            this.gameObject.SetActive(isAction);
        }
    }
}