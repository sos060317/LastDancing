using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : EnemyBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if(target == null)
            return;

        base.Update();
        Attack();
    }

    protected override void FixedUpdate()
    {
        if (target == null)
            return;

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
