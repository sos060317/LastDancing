using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float respwanTime = 10.0f;

    [SerializeField] private Image respwanBackground;
    [SerializeField] private TMP_Text respwanTimer;

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

            if(curHealth <= 0)
            {
                StartCoroutine(PlayerRespwan());
            }
        }
    }

    IEnumerator PlayerRespwan()
    {
        this.gameObject.SetActive(false);
        respwanBackground.gameObject.SetActive(true);

        while(respwanTime >= 0)
        {
            respwanTime -= Time.deltaTime;
            respwanTimer.text = ((int)respwanTime).ToString();
            yield return null;
        }

        respwanBackground.gameObject.SetActive(false);
        curHealth = maxHealth;
        respwanTime = 10.0f;
        this.gameObject.SetActive(true);

        yield break;
    }
}