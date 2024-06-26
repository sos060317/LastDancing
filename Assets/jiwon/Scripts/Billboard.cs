using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera myCamera;

    private void Start()
    {
        // 플레이어의 카메라를 찾음
        Camera[] cameras = FindObjectsOfType<Camera>();

        myCamera = cameras.FirstOrDefault(cam => {
            PhotonView PV = cam.GetComponent<PhotonView>();
            return PV != null && PV.IsMine;
        });
    }

    private void Update()
    {
        // 플레이어 쪽을 바라봄
        transform.LookAt(myCamera.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
