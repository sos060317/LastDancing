using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    private float attackDamage;

    Vector3 pos;

    private void Start()
    {
        // 위치 조정
        pos = transform.position;
        pos.y -= 1.3f;

        transform.position = pos;

        StartCoroutine(AttackMove());
    }

    public void Init(float damage)
    {
        attackDamage = damage;
    }

    private IEnumerator AttackMove()
    {
        float temp = 0f;

        while (temp < 1)
        {
            pos.y = curve.Evaluate(temp);

            transform.position = pos;

            temp += Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemyBase))
        {
            enemyBase.TakeDamage(attackDamage);
        }
    }
}