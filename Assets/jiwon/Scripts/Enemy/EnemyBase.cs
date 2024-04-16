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
        health.onDie += DieAction; // ï¿½ï¿½ï¿½ß¿ï¿½ OnEnableï¿½ï¿½ ï¿½Å±ï¿½ï¿?

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
        Debug.Log("ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿?");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}