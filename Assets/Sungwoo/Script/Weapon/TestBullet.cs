using UnityEngine;
using Photon.Pun;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private GameObject hitEfffect;

    private Rigidbody rigid;
    private PhotonView PV;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    public void Init(float spread)
    {
        rigid = GetComponent<Rigidbody>();

        rigid.velocity = (transform.forward + (Random.insideUnitSphere * spread)) * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.TryGetComponent(out EnemyBase enemyBase))
        //{
        //    enemyBase.TakeDamage(damage);
        //}

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBase>().TakeDamage(damage);
        }

        Instantiate(hitEfffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}