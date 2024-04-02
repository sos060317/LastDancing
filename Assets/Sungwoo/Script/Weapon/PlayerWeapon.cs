using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private TestBullet bulletPrefab;
    [SerializeField] private Transform shotPos;

    private float fireTimer = 0f;

    private bool isFiring;

    private void Update()
    {
        InputUpdate();
        FireUpdate();
    }

    private void InputUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            isFiring = true;
        }
        else
        {
            isFiring = false;
        }
    }

    private void FireUpdate()
    {
        // ÃÑ¾Ë ¹ß»ç
        if (fireTimer >= fireRate && isFiring)
        {
            ShotBullet();
            fireTimer = 0;
        }

        fireTimer += Time.deltaTime;
    }

    private void ShotBullet()
    {
        Instantiate(bulletPrefab, shotPos.position, transform.rotation);
    }
}