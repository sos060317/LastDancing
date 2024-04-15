using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : EnemyBase
{
    protected override void Update()
    {
        base.Update();
        Attack();
    }

    protected override void Attack()
    {
        if (!isAttackReady || shootTimer <= shootDelay)
            return;

        Vector3 shootDirection = (target.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.LookRotation(shootDirection) * Quaternion.Euler(90, 0, 0));
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * shootSpeed;

        anim.SetTrigger("shooting");

        shootTimer = 0;
    }
}
