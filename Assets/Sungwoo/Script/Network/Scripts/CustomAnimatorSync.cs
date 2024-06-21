using Photon.Pun;
using UnityEngine;

public class CustomAnimatorSync : MonoBehaviour, IPunObservable
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (anim == null)
            {
                return;
            }

            // 현재 애니메이터 파라미터 값을 네트워크에 보냅니다.
            stream.SendNext(anim.GetBool("isWalk"));
            stream.SendNext(anim.GetBool("isAttacking"));
            stream.SendNext(anim.GetFloat("InputX"));
            stream.SendNext(anim.GetFloat("InputY"));
        }
        else
        {
            if (anim == null)
            {
                return;
            }

            // 네트워크로부터 애니메이터 파라미터 값을 받습니다.
            anim.SetBool("isWalk", (bool)stream.ReceiveNext());
            anim.SetBool("isAttacking", (bool)stream.ReceiveNext());
            anim.SetFloat("InputX", (float)stream.ReceiveNext());
            anim.SetFloat("InputY", (float)stream.ReceiveNext());
        }
    }
}