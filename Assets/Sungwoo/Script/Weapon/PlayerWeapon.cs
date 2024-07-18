using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Photon.Pun;
using System.Collections;
using TMPro;

public class PlayerWeapon : MonoBehaviour, IPunObservable
{
    [SerializeField] private float fireRate;
    [SerializeField] private TestBullet bulletPrefab;
    [SerializeField] private Transform shotPos;
    [SerializeField] private Rig aimingRigLayer;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private AudioClip sound;
    [SerializeField] public int maxMagazine;
    [SerializeField] private float reloadTime = 1;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private TextMeshProUGUI magazineText;

    public bool isReload = false;

    [HideInInspector] public int curMagazine;

    private float fireTimer = 0f;
    private float bulletSpread = 0.05f;

    private bool isFiring;
    private bool isStop;

    private WeaponRecoil recoil;
    private PhotonView PV; // 플레이어 동기화

    private void Start()
    {
        // 변수 초기화
        PV = GetComponentInParent<PhotonView>();

        if (!PV.IsMine)
        {
            return;
        }

        curMagazine = maxMagazine;

        recoil = GetComponent<WeaponRecoil>();
        recoil.playerCamera = playerCamera;

        magazineText = GameManager.Instance.magazineText;
        magazineText.text = curMagazine + " / " + maxMagazine;
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

        InputUpdate();
        FireUpdate();
    }

    private void InputUpdate()
    {
        #region 좌클릭 입력부분

        if (Input.GetMouseButton(0))
        {
            isFiring = true;

            fireTimer += Time.deltaTime;
        }
        else
        {
            isFiring = false;
        }

        #endregion

        #region 우클릭 입력부분

        if (Input.GetMouseButton(1))
        {
            bulletSpread = 0.01f;
        }
        else
        {
            bulletSpread = 0.05f;
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.R) && !isReload)
        {
            if (PV.IsMine)
            {
                SoundManager.Instance.PlaySound(reloadSound, transform.position, 1);
                StartCoroutine(ReloadRoutine());
            }
        }
    }

    private void FireUpdate()
    {
        // 총알 발사
        // 조준 애니메이션 실행후 발사하긴위한 aimingRigLayer.weight >= 1
        if (fireTimer >= fireRate && isFiring && aimingRigLayer.weight >= 1 && !isReload) 
        {
            //ShotBullet();
            PV.RPC(nameof(ShotBullet), RpcTarget.All);
            fireTimer = 0;
        }
    }

    //private void ShotBullet()
    //{
    //    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", bulletPrefab.name)
    //        , shotPos.position, transform.rotation)
    //        .GetComponent<TestBullet>().Init(bulletSpread);

    //    recoil.GenerateRecoil();

    //    shootEffect.Emit(30);
    //}

    [PunRPC]
    private void ShotBullet()
    {
        Instantiate(bulletPrefab, shotPos.position, transform.rotation).Init(bulletSpread);

        curMagazine--;

        if (PV.IsMine)
        {
            recoil.GenerateRecoil();
            SoundManager.Instance.PlaySound(sound, transform.position, Random.Range(0.85f, 1.1f));

            if (curMagazine <= 0)
            {
                SoundManager.Instance.PlaySound(reloadSound, transform.position, 1);
                StartCoroutine(ReloadRoutine());
            }

            magazineText.text = curMagazine + " / " + maxMagazine;
        }

        shootEffect.Emit(30);
    }
    
    private IEnumerator ReloadRoutine()
    {
        isReload = true;

        yield return YieldInstructionCache.WaitForSeconds(reloadTime);

        isReload = false;

        curMagazine = maxMagazine;
        magazineText.text = curMagazine + " / " + maxMagazine;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.curMagazine);
            stream.SendNext(this.maxMagazine);
        }
        else if (stream.IsReading)
        {
            try
            {
                curMagazine = (int)stream.ReceiveNext();
                maxMagazine = (int)stream.ReceiveNext();
            }
            catch
            {

            }
        }
    }
}