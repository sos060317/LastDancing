using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.onDie += DieAction; // 나중에 OnEnable에 옮기기
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            health.TakeDamage(27);
        }
    }

    private void DieAction()
    {
        health.onDie -= DieAction;
        Debug.Log("권총 사망");
    }
}
