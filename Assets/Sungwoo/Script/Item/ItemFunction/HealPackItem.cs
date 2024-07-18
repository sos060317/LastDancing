using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPackItem : ItemFunctionBase
{
    public override void Init(ItemDetails details)
    {
        itemDetails = details;
    }

    public override void Upgrade()
    {
        GameManager.Instance.curPlayer.GetComponent<PlayerHealth>().Heal(30);
    }
}