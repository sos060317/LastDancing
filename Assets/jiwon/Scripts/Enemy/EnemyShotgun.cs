using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotgun : EnemyBase
{
    public int pelletCount;     // 발사할 총알 수
    public float spreadAngle; // 산탄 퍼짐 각도

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

        CircleDelayShot(10);
        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }

    /// <summary>
    /// 사방(원 모양) 탄막
    /// </summary>
    /// <param name="count"></param>
    private void CircleShot(int count)
    {
        for (int i = 0; i < 360; i += 360 / count)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, i, 90));
        }
    }

    /// <summary>
    /// 사방(원 모양)으로 흩뿌리는 탄막
    /// </summary>
    /// <param name="count"></param>
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

    /// <summary>
    /// 타겟 설정 탄막
    /// </summary>
    private void TargetingSingleShot()
    {
        // 방향 계산
        var vector = target.position - firePoint.position;
        float y = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(90, y, 0);

        // 위치와 방향으로 적용하여 생성
        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    /// <summary>
    /// 원 모양 탄막
    /// </summary>
    /// <param name="count"></param>
    private void MovingCircleShot(int count)
    {
        float radius = 2f;
        for (int i = 0; i < 360; i += 360 / count)
        {
            // 탄막을 원 모양으로 나가기 위한 위치 계산
            Vector3 pos = firePoint.position +
                new Vector3(Mathf.Cos(i * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(i * Mathf.Deg2Rad) * radius);

            // 방향 계산
            Vector3 nor = (target.position - firePoint.position).normalized;
            float y = Mathf.Atan2(nor.x, nor.z) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(90, y, 0);

            // 위치와 방향으로 적용하여 생성
            Instantiate(bulletPrefab, pos, rot);
        }
    }
}
