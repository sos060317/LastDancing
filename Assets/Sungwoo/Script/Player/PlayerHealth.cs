using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float curHealth;

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
        }
    }
}