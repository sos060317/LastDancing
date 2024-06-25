using UnityEngine;
using Photon.Pun;

public abstract class ItemFunctionBase : MonoBehaviour
{
    [HideInInspector] public PhotonView pv;
    [SerializeField] protected ItemDetails itemDetails;

    public abstract void Init(ItemDetails details);

    public abstract void Upgrade();
}