using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyShotgun : EnemyBase
{
    private Transform myGun;
    private Vector3 attackPosition = new Vector3(0, 15, 0);
    private Quaternion attackRotate = Quaternion.Euler(90, 0, 0);

    WaitForSeconds wait = new WaitForSeconds(0.1f);

    private void Awake()
    {
        myGun = transform.GetChild(0);
        Debug.Log(myGun.name);
    }

    protected override void Update()
    {
        base.Update();
        Attack();
    }

    protected override void Attack()
    {
        if (!isAttackReady || shootTimer <= shootDelay)
            return;

        isAttackReady = false;
        StartCoroutine(MoveToAttackPoint());
    }

    IEnumerator MoveToAttackPoint()
    {
        yield return wait;

        while (myGun.transform.position != target.position + attackPosition || myGun.transform.rotation != attackRotate)
        {
            myGun.transform.position = Vector3.MoveTowards(myGun.transform.position, target.position + attackPosition, 0.5f);
            myGun.transform.rotation = Quaternion.RotateTowards(myGun.transform.rotation, attackRotate, 0.5f);

            yield return wait;
        }

        Vector3 shootDirection = (target.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.LookRotation(shootDirection));
        bullet.GetComponent<Rigidbody>().velocity = shootDirection * shootSpeed;

        anim.SetTrigger(ShootingHash);

        shootTimer = 0;
        isAttackReady = true;
    }
}
