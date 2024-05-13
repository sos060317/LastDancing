using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Init(float spread)
    {
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = (transform.forward + (Random.insideUnitSphere * spread)) * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}