using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : EnemyBase
{
    public int pelletCount;     // ¹ß»çÇÒ ÃÑ¾Ë ¼ö
    public float spreadAngle; // »êÅº ÆÛÁü °¢µµ

    private void Update()
    {
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

        AllDirectionRotationsShot(10, 10);
        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }
}
