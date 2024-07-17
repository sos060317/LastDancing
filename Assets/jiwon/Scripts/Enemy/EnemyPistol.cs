using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : EnemyBase
{
    [SerializeField] private AudioClip shotSound;

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
        TargetingSingleShot();

        SoundManager.Instance.PlaySound(shotSound, transform.position, 1);

        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }
}
