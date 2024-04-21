using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeItem : ItemFunctionBase
{
    [SerializeField] private GameObject grenadePrefab;

    private int curLevel;

    private bool isAttacking;

    public override void Init(ItemDetails details)
    {
        this.itemDetails = details;
    }

    public override void Upgrade()
    {
        curLevel = Mathf.Min(curLevel + 1, itemDetails.itemData.Length - 1);

        if (!isAttacking)
        {
            StartCoroutine(AttackRoutine());
            isAttacking = true;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {

        }
    }
}