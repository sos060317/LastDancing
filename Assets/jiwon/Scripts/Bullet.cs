using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;

    private float moveSpeed = 10f;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.up);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") ||
           collision.gameObject.CompareTag("Object") ||
           collision.gameObject.CompareTag("Ground"))
        {
            if (collision.transform.TryGetComponent(out PlayerHealth health))
            {
                health.OnDamage(damage);
            }

            Debug.Log("hit!");
            Destroy(gameObject);
        }
    }
}