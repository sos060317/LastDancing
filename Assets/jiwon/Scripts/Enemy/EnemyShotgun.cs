using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : EnemyBase
{
    public int pelletCount;     // 발사할 총알 수
    public float spreadAngle; // 산탄 퍼짐 각도

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

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

        // 고유 공격 패턴
        SpreadShot(pelletCount, spreadAngle);

        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }
}
