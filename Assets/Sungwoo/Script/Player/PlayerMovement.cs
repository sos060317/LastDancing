using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private CinemachineBrain brain;
    [SerializeField] private Transform cameraLookAt;

    private float turnSpeedMultiplier;

    private bool isWalk;
    private bool isAttacking;
    private bool isStop;

    private Animator anim;
    [SerializeField] private PhotonView PV; // 플레이어 동기화

    private Vector2 inputVec;
    private Vector2 moveVec;
    private Vector2 velocity;
    private Vector3 targetDirection;
    private Quaternion freeRotation;

    private void Start()
    {
        if (!PV.IsMine)
        {
            mainCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            Destroy(cinemachine.transform.gameObject);
            Destroy(brain);
            return;
        }

        if (PV.IsMine)
        {
            cinemachine.Follow = transform;
            cinemachine.LookAt = cameraLookAt;

            anim = GetComponent<Animator>();

            // 커서 숨기기
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
        if (!PV.IsMine)
        {
            return;
        }
        // 커서 보이기
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 애니메이션 멈추기
        anim.StartPlayback();

        isStop = true;
    }

    private void TimePlay()
    {
        if (!PV.IsMine)
        {
            return;
        }
        // 커서 숨기기
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 애니메이션 재생
        anim.StopPlayback();

        isStop = false;
    }

    private void FixedUpdate()
    {
        if (isStop || !PV.IsMine)
        {
            return;
        }

        InputUpdate();
        AnimationUpdate();
        TargetDirectionUpdate();
        RotationUpdate();
    }

    private void InputUpdate()
    {
        #region 이동 키 입력 로직

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        moveVec = Vector2.SmoothDamp(moveVec, inputVec, ref velocity, 0.05f);

        if (inputVec != Vector2.zero)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        #endregion

        #region 공격키 입력 로직

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        #endregion 
    }

    private void AnimationUpdate()
    {
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetFloat("InputX", moveVec.x);
        anim.SetFloat("InputY", moveVec.y);
    }

    private void TargetDirectionUpdate()
    {
        // 마우스를 누르고 있지 않으면 이동방향으로 회전
        if (!isAttacking)
        {
            turnSpeedMultiplier = 1f;
            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            var right = mainCamera.transform.TransformDirection(Vector3.right);

            targetDirection = inputVec.x * right + inputVec.y * forward;
        }
        // 마우스를 누르고 있으면 카메라가 보는 방향으로 회전
        else
        {
            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            targetDirection = forward;
        }
    }

    private void RotationUpdate()
    {
        if (isWalk || isAttacking)
        {
            Vector3 lookDirection = targetDirection.normalized;
            freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
            var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;

            if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
            var euler = new Vector3(0, eulerY, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        }
    }
}