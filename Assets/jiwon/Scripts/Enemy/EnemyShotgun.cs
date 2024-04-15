using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : EnemyBase
{
    [Space(10)]
    public float speed;

    private Transform myGun;

    private Vector3 attackPosition;

    protected override void Start()
    {
        base.Start();

        myGun = transform.GetChild(0);
        attackPosition = new Vector3(0, 10, 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Attack();
    }

    protected override void Attack()
    {
        if (!isAttackReady || shootTimer <= shootDelay)
            return;

        shootTimer = 0;

        StartCoroutine(MoveToAttack());

        // 총 쏘는 로직
        Vector3 shootDirection = (target.position - myGun.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.LookRotation(shootDirection) * Quaternion.Euler(90, 0, 0));
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * shootSpeed;

        anim.SetTrigger(ShootingHash);
    }

    IEnumerator MoveToAttack()
    {
        // 플레이어 머리 위로 이동하는 로직
        anim.enabled = false;

        while(myGun.position != target.position + attackPosition)
        {
            myGun.position = Vector3.MoveTowards(myGun.position, target.position + attackPosition, speed * Time.deltaTime);
            yield return null;
        }

        myGun.LookAt(target.position);
    }

}
