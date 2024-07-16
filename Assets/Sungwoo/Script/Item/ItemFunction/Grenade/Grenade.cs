using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Grenade miniGrenade;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask enemyLayer;

    private int miniGrenadeCount = 0;

    private float explosionTime = 3f;
    private float explosionDamage;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
        
    private IEnumerator ExplosionRoutine()
    {
        yield return YieldInstructionCache.WaitForSeconds(explosionTime);

        // 폭발
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Effects", "ExplosionEffect"), transform.position, Quaternion.identity)
            .transform.localScale = transform.localScale / 2;

        // 미니 수류탄 생성
        for (int i = 0; i < miniGrenadeCount; i++)
        {
            var grenade = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ItemPrefab", "MiniGrenade"),
                transform.position + new Vector3(Random.Range(-0.4f, 0.4f), 0.3f, Random.Range(-0.4f, 0.4f)), Quaternion.identity)
                .GetComponent<Grenade>();

            grenade.Init(transform.position, 200f, explosionDamage / 3);
        }

        // 적들 데미지 주기 
        Collider[] enemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out EnemyBase enemyBase))
            {
                enemyBase.TakeDamage(explosionDamage);
            }
        }

        PhotonNetwork.Destroy(gameObject);
    }

    public void Init(int grenadeCount, float damage)
    {
        rigid = GetComponent<Rigidbody>();
        explosionDamage = damage;

        // 날리기
        rigid.AddForce(new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1f, 1f)) * 4, ForceMode.Impulse);

        Debug.Log("aa");

        miniGrenadeCount = grenadeCount;

        StartCoroutine(ExplosionRoutine());
    }

    /// <summary>
    /// 미니 수류탄 초기화 함수
    /// </summary>
    public void Init(Vector3 point, float force, float damage)
    {
        rigid = GetComponent<Rigidbody>();
        explosionDamage = damage;

        rigid.AddExplosionForce(force, point, 100f);

        miniGrenadeCount = 0;

        StartCoroutine(ExplosionRoutine());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}