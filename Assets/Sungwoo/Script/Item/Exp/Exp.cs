using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float scanRange;
    [SerializeField] private float moveSpeed;

    private Rigidbody rigid;
    private Collider col;

    private Transform target = null;

    PhotonView PV;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        PV = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        MoveToTarget();
        CheckPlayer();
    }

    private void MoveToTarget()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.position;
        targetPos.y += 1;

        Vector3 dir = targetPos - transform.position;

        rigid.velocity = dir.normalized * moveSpeed;
    }

    private void CheckPlayer()
    {
        if (target != null)
        {
            return;
        }

        var targets = Physics.OverlapSphere(transform.position, scanRange, playerLayer);

        float dis = 9999999;

        if (targets.Length != 0)
        {
            foreach (var temp in targets)
            {
                if (Vector3.Distance(transform.position, temp.transform.position) < dis)
                {
                    target = temp.transform;
                }
            }

            rigid.useGravity = false;
            col.isTrigger = true;
        }
    }

    public void DestroyEXPPrefab()
    {
        PV.RPC(nameof(RPC_DestroyEXPPrefab), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_DestroyEXPPrefab()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}
