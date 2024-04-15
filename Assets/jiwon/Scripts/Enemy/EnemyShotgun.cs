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

        // »êÅº ÃÑ¾Ë ¹ß»ç
        for (int i = 0; i < pelletCount; i++)
        {
            // ÃÑ¾ËÀÌ ³ª°¥ °¢µµ °è»ê
            float randomSpread1 = Random.Range(0, -spreadAngle);
            float randomSpread2 = Random.Range(0, -spreadAngle);
            Quaternion bulletDirection = Quaternion.Euler(90 + randomSpread1, randomSpread2, 0);

            // ÃÑ¾Ë »ý¼º
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletDirection);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * fireSpeed;
        }

        fireTimer = 0;
    }
}
