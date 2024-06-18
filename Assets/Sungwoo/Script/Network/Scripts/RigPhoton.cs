using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class RigPhoton :  MonoBehaviour, IPunObservable
{
    [SerializeField] private Rig rig;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 자신이면 데이터 보내기
        if (stream.IsWriting)
        {
            stream.SendNext(rig.weight);
        }
        // 자신이 아니면 데이터 받기
        else if (stream.IsReading)
        {
            rig.weight = (float)stream.ReceiveNext();
        }
    }
}