using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Grenade miniGrenade;
    [SerializeField] private GameObject explosionEffect;

    private int miniGrenadeCount;

    private float explosionTime = 5f;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();

        StartCoroutine(ExplosionRoutine());
    }

    private IEnumerator ExplosionRoutine()
    {
        // ³¯¸®±â
        rigid.AddForce(new Vector3(Random.Range(-1f, 1f) ,1 , Random.Range(-1f, 1f)) * Random.Range(3, 5), ForceMode.Impulse);

        yield return YieldInstructionCache.WaitForSeconds(explosionTime);

        // Æø¹ß
        Instantiate(explosionEffect, transform.position, Quaternion.identity).transform.localScale = transform.localScale / 2;
    }

    public void Init(int grenadeCount)
    {
        miniGrenadeCount = grenadeCount;
    }
}