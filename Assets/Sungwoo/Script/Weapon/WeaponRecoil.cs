using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WeaponRecoil : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera playerCamera;
    [HideInInspector] public CinemachineImpulseSource cameraShake;

    public Vector2[] recoilPattern;
    public float duration;

    private int recoilIndex = 0;

    private float verticalRecoil;
    private float horizontalRecoil;
    private float time = 0;

    private CinemachinePOV aim;

    private void Start()
    {
        // 변수 할당
        aim = playerCamera.AddCinemachineComponent<CinemachinePOV>();
        cameraShake = GetComponent<CinemachineImpulseSource>();
    }

    private int NextIndex()
    {
        return (recoilIndex + 1) % recoilPattern.Length;
    }

    public void GenerateRecoil()
    {
        time = duration;

        horizontalRecoil = recoilPattern[recoilIndex].x;
        verticalRecoil = recoilPattern[recoilIndex].y;

        recoilIndex = NextIndex();

        // 시네머신 쉐이크
        cameraShake.GenerateImpulse();
    }

    private void Update()
    {
        // 반동 주기
        if (time > 0)
        {
            aim.m_VerticalAxis.Value -= (verticalRecoil * Time.deltaTime) / duration;
            aim.m_HorizontalAxis.Value -= (horizontalRecoil * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}