using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private float range;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform target;

    private Health health;
    private Rigidbody rigid;
    private NavMeshAgent agent;

    private void Start()
    {
        health = GetComponent<Health>();
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        health.onDie += DieAction; // 나중에 OnEnable에 옮기기
    }

    private void Update()
    {
        if (target == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            health.TakeDamage(27);
        }
        Move();
    }

    protected abstract void Attack();

    private void Move()
    {
        if (Vector3.Distance(transform.position, target.position) <= range)
        {

            agent.speed = 0f;
        }

        agent.SetDestination(target.position);
    }

    private void DieAction()
    {
        health.onDie -= DieAction;
        Debug.Log("권총 사망");
    }
}