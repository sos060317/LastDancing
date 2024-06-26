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

        StartCoroutine(aasdf());
    }

    private void Update()
    {
        //Attack();
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
        //AllDirectionRotationsShot(10, 10);

        anim.SetTrigger(ShootingHash);

        fireTimer = 0;
    }

    IEnumerator aasdf()
    {
        CircleShot(10);
        yield return new WaitForSeconds(3f);
        CircleCircleShapeShot(10, 5);
        yield return new WaitForSeconds(3f);
        CircleDelayShot(10);
        yield return new WaitForSeconds(3f);
        CircleDelayCircleShapeShot(10, 5);
        yield return new WaitForSeconds(3f);
        TargetingSingleShot();
        yield return new WaitForSeconds(3f);
        TargetingCircleShapeShot(10);
        yield return new WaitForSeconds(3f);
        SpreadShot(10, 30);
        yield return new WaitForSeconds(3f);
        AllDirectionRotationsShot(10, 10);
        yield return new WaitForSeconds(3f);
    }
}
