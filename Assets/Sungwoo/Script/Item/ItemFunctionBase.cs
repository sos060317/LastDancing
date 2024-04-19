using UnityEngine;

public abstract class ItemFunctionBase : MonoBehaviour
{
    protected ItemDetails itemDetails;

    public abstract void Init(ItemDetails details);

    public abstract void Upgrade();
}