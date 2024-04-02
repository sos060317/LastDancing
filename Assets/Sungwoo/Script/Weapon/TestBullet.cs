using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        Init();
    }

    public void Init()
    {
        rigid.velocity = transform.forward * moveSpeed;
    }
}