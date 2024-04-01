using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class EnemyBase : MonoBehaviour
{
    [System.Serializable]
    public struct FieldOfView
    {
        [Tooltip("반지름")]
        public float lookRadius;

        [Range(0, 360), Tooltip("시야각")]
        public float angle;

        [Space(10f)]
        [Tooltip("타겟")] 
        public LayerMask targetMask;

        [Tooltip("장애물")] 
        public LayerMask obstructionMask;

        [HideInInspector]
        public bool canSeePlayer;    // 플레이어를 보고있는지 아닌지를 참거짓으로 확인
                                     
        [HideInInspector]            
        public GameObject playerRef; // 플레이어 오브젝트
    }

    [Header("Enemy Base"), Tooltip("적 시야각 설정")]
    public FieldOfView fieldOfView;

    [SerializeField] private GameObject bulletPrefab;

    private Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.onDie += DieAction; // 나중에 OnEnable에 옮기기

        StartCoroutine(FOVRoutine()); // 코루틴 호출
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            health.TakeDamage(27);
        }
    }

    private void Move()
    {

    }

    private void DieAction()
    {
        health.onDie -= DieAction;
        Debug.Log("권총 사망");
    }

    private void FieldOfViewCheck()
    {
        // radius값을 반지름으로 한 원 안에 특정 레이어(targetMask)를 가지고 있는 충돌체를 감지하여 배열에 저장함
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fieldOfView.lookRadius, fieldOfView.targetMask);

        if (rangeChecks.Length != 0)                     // 넓은 범위에 충돌체가 있으면
        {
            Transform target = rangeChecks[0].transform; // 넓은 범위에 첫번째로 충돌된 충돌체의 위치를 저장함

            // 두 벡터의 위치 값을 빼고 정규화를 통해서 방향 벡터를 1로 하는 단위 벡터를 저장함
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // 내 전방과 directionToTarget사이의 값이 angle의 절반보다 작으면 if문 실행
            if (Vector3.Angle(transform.forward, directionToTarget) < fieldOfView.angle / 2)
            {
                // 내 위치와 target의 위치를 저장함
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // 현재 위치에서 directionToTarget의 방향으로 distanceToTarget거리 만큼을 검사하며
                // obstructionMask가 아니면 아마 targetMask이면 if문을 실행
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, fieldOfView.obstructionMask))
                {
                    fieldOfView.canSeePlayer = true;    // 플레이어를 봄
                }                                       
                else                                    
                {                                       
                    fieldOfView.canSeePlayer = false;   // 플레이어를 못 봄
                }                                       
            }                                           
            else                                        
            {                                           
                fieldOfView.canSeePlayer = false;       // 플레이어를 못 봄
            }                                           
        }                                               
        else if (fieldOfView.canSeePlayer)              
        {                                               
            fieldOfView.canSeePlayer = false;           // 플레이어를 못 봄
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); // 0.2초 변수 저장

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
}
