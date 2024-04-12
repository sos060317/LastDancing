using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private TestBullet bulletPrefab;
    [SerializeField] private Transform shotPos;
    [SerializeField] private Rig aimingRigLayer;
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    private float fireTimer = 0f;
    private float bulletSpread = 0.05f;

    private bool isFiring;

    private WeaponRecoil recoil;

    private void Start()
    {
        // 변수 초기화
        recoil = GetComponent<WeaponRecoil>();
        recoil.playerCamera = playerCamera;
    }

    private void Update()
    {
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
    }

    private void FireUpdate()
    {
        // 총알 발사
        if (fireTimer >= fireRate && isFiring && aimingRigLayer.weight >= 1) // 조준 애니메이션 실행후 발사하긴위한 aimingRigLayer.weight >= 1
        {
            ShotBullet();
            fireTimer = 0;
        }
    }

    private void ShotBullet()
    {
        Instantiate(bulletPrefab, shotPos.position, transform.rotation).Init(bulletSpread);

        recoil.GenerateRecoil();
    }
}