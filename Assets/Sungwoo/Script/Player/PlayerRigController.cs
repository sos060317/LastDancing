using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigController : MonoBehaviour
{
    [SerializeField] private float aimDuration = 0.3f;
    [SerializeField] private Rig aimingRigLayer;
    [SerializeField] private Rig bodyRigLayer;

    private void Update()
    {
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