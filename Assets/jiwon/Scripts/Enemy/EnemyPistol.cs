using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : EnemyBase
{
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // 마스터 클라이언트가 아니면 실행하지 않음
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (target == null)
            return;

        Attack();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack()
    {
        if (!isAttackReady || fireTimer <= fireDelay)
            return;

        TargetingSingleShot();

        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }
}
