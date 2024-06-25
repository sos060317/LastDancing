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
        Camera[] cameras = FindObjectsOfType<Camera>();

        myCamera = cameras.FirstOrDefault(cam => {
            PhotonView PV = cam.GetComponent<PhotonView>();
            return PV != null && PV.IsMine;
        });
    }

    private void Update()
    {
        transform.LookAt(myCamera.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
