using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigController : MonoBehaviour
{
    [SerializeField] private float aimDuration = 0.3f;
    [SerializeField] private Rig aimingRigLayer;
    [SerializeField] private Rig bodyRigLayer;
    [SerializeField] private PlayerWeapon weapon;

    private bool isStop;

    private PhotonView PV; // 플레이어 동기화

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        // 시간 멈춤 이벤트 등록
        TimeManager.Instance.TimeStopAction += TimeStop;
        TimeManager.Instance.TimePlayAction += TimePlay;
    }

    private void OnDisable()
    {
        // 시간 멈춤 이벤트 등록 해제
        TimeManager.Instance.TimeStopAction -= TimeStop;
        TimeManager.Instance.TimePlayAction -= TimePlay;
    }

    private void TimeStop()
    {
        isStop = true;
    }

    private void TimePlay()
    {
        isStop = false;
    }

    private void Update()
    {
        if (isStop || !PV.IsMine)
        {
            return;
        }

        AimingUpdate();
    }

    private void AimingUpdate()
    {
        // 마우스를 누르면 애니메이션 전환
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            aimingRigLayer.weight += Time.deltaTime / aimDuration;
            bodyRigLayer.weight += Time.deltaTime / aimDuration;
        }
        else
        {
            aimingRigLayer.weight -= Time.deltaTime / aimDuration;
            bodyRigLayer.weight -= Time.deltaTime / aimDuration;
        }
    }
}