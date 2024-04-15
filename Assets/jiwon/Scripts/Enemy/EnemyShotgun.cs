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

        TargetingSingleShot();
        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }

    private void CircleShot(int count)
    {
        for (int i = 0; i < 360; i += 360 / count)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, i, 90));
        }
    }

    private void CircleDelayShot(int count)
    {
        StartCoroutine(shotCoroutine());

        IEnumerator shotCoroutine()
        {
            for (int i = 0; i < 360; i += 360 / count)
            {
                Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, i, 90));
                yield return new WaitForSeconds(0.1f);
            }

            yield break;
        }
    }

    private void TargetingSingleShot()
    {
        float z = Mathf.Atan2(target.position.z, target.position.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0, z, 90);
        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    private void MovingCircleShot(int count)
    {
        float radius = 2f;
        for (int i = 0; i < 360; i += 360 / count)
        {
            Vector3 pos = firePoint.position + new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(i * Mathf.Deg2Rad) * radius);

            Vector3 nor = (target.position - firePoint.position).normalized;
            float y = Mathf.Atan2(nor.z, nor.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, y, 90);

            Instantiate(bulletPrefab, pos, rot);
        }
    }
}
