using UnityEngine;
using Photon.Pun;

public class CustomTransformView : MonoBehaviour, IPunObservable
{
    [SerializeField] private float smoothAmount = 10f;

    private PhotonView pv;

    private Vector3 currentPos;
    private Quaternion rotate;

    private void Awake()
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
            if (currentPos != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, currentPos, Time.smoothDeltaTime * smoothAmount);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.smoothDeltaTime * smoothAmount);
            }
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
            try
            {
                currentPos = (Vector3)stream.ReceiveNext();
                rotate = (Quaternion)stream.ReceiveNext();
            }
            catch
            {

            }
        }
    }
}