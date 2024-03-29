using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Camera mainCamera;

    private float turnSpeedMultiplier;

    private bool isWalk;

    private Animator anim;

    private Vector2 inputVec;
    private Vector3 targetDirection;
    private Quaternion freeRotation;

    private void Start()
    {
        anim = GetComponent<Animator>();

        //임시
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
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

        if (inputVec != Vector2.zero)
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }

        #endregion
    }

    private void AnimationUpdate()
    {
        anim.SetBool("isWalk", isWalk);
    }

    private void TargetDirectionUpdate()
    {
        turnSpeedMultiplier = 1f;
        var forward = mainCamera.transform.TransformDirection(Vector3.forward);
        forward.y = 0;

        var right = mainCamera.transform.TransformDirection(Vector3.right);

        targetDirection = inputVec.x * right + inputVec.y * forward;
    }

    private void RotationUpdate()
    {
        if (isWalk && targetDirection.magnitude > 0.1f)
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