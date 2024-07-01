using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.IO;

// 적 기본 로직
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviourPun
{
    [Header("Enemy Base")] // 적 기본 스탯
    [SerializeField] protected float range;
    [SerializeField] protected float firstScanRange;
    [SerializeField] protected float currentScanRange;
    [SerializeField] protected float fireSpeed;
    [SerializeField] protected float fireTimer;
    [SerializeField] protected float fireDelay;
    [SerializeField] protected bool isAttackReady;

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject dieEffect;
    [SerializeField] protected GameObject expPrefab;
    [SerializeField] protected Transform firePoint;

    [SerializeField] protected LayerMask targetLayer;

    protected Collider[] targets;
    [SerializeField] protected Transform target;

    protected Health health;
    protected Animator anim;
    protected NavMeshAgent agent;
    protected PhotonView PV;

    protected readonly int ShootingHash = Animator.StringToHash("shooting");

    protected virtual void Start()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        PV = GetComponent<PhotonView>();
        health.onDie += DieAction; // 오브젝트 풀링 제작 후 인에이블로 옮기기

        fireTimer = fireDelay;
        currentScanRange = firstScanRange;
    }

    protected virtual void FixedUpdate()
    {
        fireTimer += Time.deltaTime;
        SearchNearPlayer();
        Move();
    }

    protected abstract void Attack();

    /// <summary>
    /// 가장 근처에 있는 플레이어 타겟팅
    /// </summary>
    private void SearchNearPlayer()
    {
        targets = Physics.OverlapSphere(transform.position, currentScanRange, targetLayer);

        if (targets.Length > 0)
        {
            currentScanRange = firstScanRange;

            if(targets[0].transform != target)
            {
                target = targets[0].transform;
                return;
            }

            target = targets[0].transform;
        }
        else
        {
            currentScanRange += 5.0f;
        }
    }

    /// <summary>
    /// 이동 로직
    /// </summary>
    private void Move()
    {
        if (target == null)
        {
            return;
        }

        if (Vector3.Distance(transform.position, target.position) <= range)
        {
            transform.LookAt(target.transform);
            agent.speed = 0;
            isAttackReady = true;
            return;
        }
        else
        {
            agent.speed = 3.5f;
            isAttackReady = false;
            agent.SetDestination(target.position);
        }
    }

    /// <summary>
    /// 죽음 액션
    /// </summary>
    private void DieAction()
    {
        health.onDie -= DieAction;
        GameManager.Instance.currentKillenemies++;

        Instantiate(dieEffect, transform.position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 4; i++)
            {
                var exp = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ExpPrefab"), transform.position + Random.insideUnitSphere, Quaternion.identity);
                exp.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 30);
            }
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// 데미지 받기 동기화
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        Gizmos.DrawWireSphere(transform.position, currentScanRange);
    }

    #region 탄막 패턴
    #region 사방 탄막
    /// <사방 원형 탄막>
    /// 사방으로 탄막을 원 모양으로 한번에 발사
    /// </summary>
    /// <param name="bulletCount"></param>
        protected void CircleShot(int bulletCount)
        {
            for (int i = 0; i < 360; i += 360 / bulletCount)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, i, 90));
        }
    }

    /// <사방 원형 원 모양 탄막>
    /// 사방으로 원 모양 탄막을 원형으로 발사
    /// </summary>
    /// <param name="circleCount"></param>
    /// <param name="bulletCount"></param>
    protected void CircleCircleShapeShot(int bulletCount, int circleCount)
    {
        float radius = 2f;
        for (int i = 0; i < 360; i += 360 / circleCount)
        {
            for (int j = 0; j < 360; j += 360 / bulletCount)
            {
                // 탄막을 원 모양으로 나가기 위한 위치 계산
                Vector3 pos = firePoint.position +
                    new Vector3(Mathf.Cos(j * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(j * Mathf.Deg2Rad) * radius);

                // 위치와 방향으로 적용하여 생성
                Instantiate(bulletPrefab, pos, Quaternion.Euler(0, i, 90));
            }
        }
    }
    #endregion

    #region 사방 흩뿌리기 탄막
    /// <사방 원형 흩뿌리기 탄막>
    /// 사방으로 탄막을 원 모양으로 순차적으로 발사
    /// </summary>
    /// <param name="bulletCount"></param>
    protected void CircleDelayShot(int bulletCount)
    {
        StartCoroutine(shotCoroutine());

        IEnumerator shotCoroutine()
        {
            for (int i = 0; i < 360; i += 360 / bulletCount)
            {
                Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, i, 90));
                yield return new WaitForSeconds(0.1f);
            }

            yield break;
        }
    }

    /// <summary>
    /// 사방 원형 원 모양 흩뿌리기 탄막
    /// </summary>
    /// <param name="circleCount"></param>
    /// <param name="bulletCount"></param>
    protected void CircleDelayCircleShapeShot(int bulletCount, int circleCount)
    {
        StartCoroutine(shotCoroutine());

        IEnumerator shotCoroutine()
        {
            float radius = 2f;
            for (int i = 0; i < 360; i += 360 / circleCount)
            {
                for (int j = 0; j < 360; j += 360 / bulletCount)
                {
                    // 탄막을 원 모양으로 나가기 위한 위치 계산
                    Vector3 pos = firePoint.position +
                        new Vector3(Mathf.Cos(j * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(j * Mathf.Deg2Rad) * radius);

                    // 위치와 방향으로 적용하여 생성
                    Instantiate(bulletPrefab, pos, Quaternion.Euler(0, i, 90));
                }

                yield return new WaitForSeconds(0.1f);
            }

            yield break;
        }
    }
    #endregion

    #region 타겟 설정 탄막
    /// <타겟 설정 탄막>
    /// 설정한 타겟을 향하여 발사
    /// </summary>
    protected void TargetingSingleShot()
    {
        // 방향 계산
        var vector = target.position - firePoint.position;
        float y = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(90, y, 0);

        // 위치와 방향으로 적용하여 생성
        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    /// <타겟 설정 원 모양 탄막>
    /// 총알을 원 모양으로 배치하여 발사
    /// </summary>
    /// <param name="bulletCount"></param>
    protected void TargetingCircleShapeShot(int bulletCount)
    {
        float radius = 2f;
        for (int i = 0; i < 360; i += 360 / bulletCount)
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
    #endregion

    #region 확산 탄막
    /// <확산 탄막>
    /// 전방에 퍼지는 탄막을 발사
    /// </summary>
    protected void SpreadShot(int bulletCount, float spreadRange)
    {
        for(int i = 0; i < bulletCount; i++)
        {
            // 방향 계산
            var vector = target.position - firePoint.position;
            float y = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(
                90 + Random.Range(-spreadRange, spreadRange),
                y,
                Random.Range(-spreadRange, spreadRange));

            // 위치와 방향으로 적용하여 생성
            Instantiate(bulletPrefab, firePoint.position, rot);
        }
    }
    #endregion

    #region 보스 탄막 패턴
    /// <summary>
    /// 전 방향 회전 탄막
    /// </summary>
    /// <param name="bulletCount"></param>
    /// <param name="rotationCount"></param>
    protected void AllDirectionRotationsShot(int bulletCount, int rotationCount)
    {
        for(int i = 0; i <= 180; i += 180 / rotationCount)
        {
            for (int j = 0; j <= 180; j += 180 / bulletCount)
            {
                Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, i, 90 - j));
            }
        }
    }
    #endregion
    #endregion
}