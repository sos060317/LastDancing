using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera playerCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;

    public float verticalRecoil;
    public float duration;

    private float time = 0;

    private CinemachinePOV aim;

    private void Start()
    {
        // 변수 할당
        aim = playerCamera.AddCinemachineComponent<CinemachinePOV>();
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    public void GenerateRecoil()
    {
        time = duration;
        cameraShake.GenerateImpulse();
    }

    private void Update()
    {
        // 반동 주기
        if (time > 0)
        {
            aim.m_VerticalAxis.Value -= (verticalRecoil * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}