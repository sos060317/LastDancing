using UnityEngine;
using Photon.Pun;

public class CustomTransformView : MonoBehaviour, IPunObservable
{
    private PhotonView pv;

    private Vector3 currentPos;
    private Quaternion rotate;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            return;
        }

        // 실제 위치보다 너무 떨어져 있으면 위치 바로 옮김
        if ((transform.position - currentPos).sqrMagnitude >= 100)
        {
            transform.position = currentPos;
            transform.rotation = rotate;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, currentPos, Time.smoothDeltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.smoothDeltaTime * 10);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.gameObject.transform.position);
            stream.SendNext(this.gameObject.transform.rotation);
        }
        else if (stream.IsReading)
        {
            currentPos = (Vector3)stream.ReceiveNext();
            rotate = (Quaternion)stream.ReceiveNext();
        }
    }
}