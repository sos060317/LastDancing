using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Grenade miniGrenade;
    [SerializeField] private GameObject explosionEffect;

    private int miniGrenadeCount = 0;

    private float explosionTime = 3f;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
        
    private IEnumerator ExplosionRoutine()
    {
        yield return YieldInstructionCache.WaitForSeconds(explosionTime);

        // 폭발
        Instantiate(explosionEffect, transform.position, Quaternion.identity).transform.localScale = transform.localScale / 2;

        // 미니 수류탄 생성
        for (int i = 0; i < miniGrenadeCount; i++)
        {
            var grenade = Instantiate(
                miniGrenade, 
                transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0.3f, Random.Range(-0.4f, 0.4f)), 
                Quaternion.identity);

            grenade.Init(transform.position, 200f);
        }
        
        // 적들 데미지 주기 
        // ...

        Destroy(gameObject);
    }

    public void Init(int grenadeCount)
    {
        rigid = GetComponent<Rigidbody>();

        // 날리기
        rigid.AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * 4, ForceMode.Impulse);

        miniGrenadeCount = grenadeCount;

        StartCoroutine(ExplosionRoutine());
    }

    /// <summary>
    /// 미니 수류탄 초기화 함수
    /// </summary>
    public void Init(Vector3 point, float force)
    {
        rigid = GetComponent<Rigidbody>();

        rigid.AddExplosionForce(force, point, 100f);

        miniGrenadeCount = 0;

        StartCoroutine(ExplosionRoutine());
    }
}