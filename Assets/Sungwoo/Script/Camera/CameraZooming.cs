using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    [SerializeField] private float defaultZoomAmount;
    [SerializeField] private float zoomingAmount;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private CinemachineCameraOffset cinemachineCameraOffset;

    private void Start()
    {
        cinemachineCameraOffset.m_Offset.z = defaultZoomAmount;
    }

    Coroutine zoomRoutine;
    private void Update()
    {
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