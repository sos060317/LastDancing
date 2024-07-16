using UnityEngine;
using Photon.Pun;

public class ExpGetter : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("EXP"))
        {
            Instantiate(particle, other.transform.position, Quaternion.identity);

            if (pv.IsMine)
            {
                EXPManager.Instance.ExpPlus();
            }

            other.gameObject.GetComponent<Exp>().DestroyEXPPrefab();
        }
    }
}