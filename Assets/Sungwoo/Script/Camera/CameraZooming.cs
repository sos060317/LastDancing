using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraZooming : MonoBehaviour
{
    [SerializeField] private float defaultZoomAmount;
    [SerializeField] private float zoomingAmount;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private CinemachineCameraOffset cinemachineCameraOffset;
    [SerializeField] private CinemachineVirtualCamera cinemachineCameraComponent;

    private bool isStop;

    private void Start()
    {
        cinemachineCameraOffset.m_Offset.z = defaultZoomAmount;
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
        // 카메라 회전 비활성화
        cinemachineCameraComponent.enabled = false;

        isStop = true;
    }

    private void TimePlay()
    {
        // 카메라 회전 활성화
        cinemachineCameraComponent.enabled = true;

        isStop = false;
    }

    Coroutine zoomRoutine;
    private void Update()
    {
        if (isStop)
        {
            return;
        }

        // 우클릭 하면 줌
        if (Input.GetMouseButtonDown(1))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
            }

            zoomRoutine = StartCoroutine(ZoomStart());
        }
        // 우클릭 때면 줌 해제
        else if (Input.GetMouseButtonUp(1))
        {
            if (zoomRoutine != null)
            {
                StopCoroutine(zoomRoutine);
            }

            zoomRoutine = StartCoroutine(ZoomEnd());
        }
    }

    private IEnumerator ZoomStart()
    {
        float curZoomAmount = cinemachineCameraOffset.m_Offset.z;

        while (curZoomAmount != zoomingAmount)
        {
            curZoomAmount = Mathf.Lerp(curZoomAmount, zoomingAmount, Time.deltaTime * zoomSpeed);
            cinemachineCameraOffset.m_Offset.z = curZoomAmount;

            yield return null;
        }
    }

    private IEnumerator ZoomEnd()
    {
        float curZoomAmount = cinemachineCameraOffset.m_Offset.z;

        while (curZoomAmount != defaultZoomAmount)
        {
            curZoomAmount = Mathf.Lerp(curZoomAmount, defaultZoomAmount, Time.deltaTime * zoomSpeed);
            cinemachineCameraOffset.m_Offset.z = curZoomAmount;

            yield return null;
        }
    }
}