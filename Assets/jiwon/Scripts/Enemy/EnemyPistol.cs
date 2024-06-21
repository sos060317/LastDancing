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
        if(target == null)
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
