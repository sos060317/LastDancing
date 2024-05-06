using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : EnemyBase
{
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

        Vector3 fireDirection = (target.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(fireDirection) * Quaternion.Euler(90, 0, 0));
        bullet.GetComponent<Rigidbody>().velocity = fireDirection * fireSpeed;

        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }
}
