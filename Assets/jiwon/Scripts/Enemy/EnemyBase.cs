using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Base")]
    [SerializeField] protected float range;
    [SerializeField] protected float fireSpeed;
    [SerializeField] protected float fireTimer;
    [SerializeField] protected float fireDelay;
    [SerializeField] protected bool isAttackReady;

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform target;

    protected Health health;
    protected Animator anim;
    protected NavMeshAgent agent;

    protected readonly int ShootingHash = Animator.StringToHash("shooting");

    protected virtual void Start()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health.onDie += DieAction; // 나중에 OnEnable에 옮기기

        fireTimer = fireDelay;
    }

    protected virtual void FixedUpdate()
    {
        if (target == null)
            return;

        fireTimer += Time.deltaTime;

        Move();
    }

    protected abstract void Attack();

    private void Move()
    {
        if (Vector3.Distance(transform.position, target.position) <= range)
        {
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

    private void DieAction()
    {
        health.onDie -= DieAction;
        Debug.Log("권총 사망");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}