using UnityEngine;
using System;

public class ItemCard : MonoBehaviour
{
    public int curLevel;

    private ItemFunctionBase item;

    private ItemDetails itemDetails;

    public void InitItemCard(ItemDetails details)
    {
        // 변수 초기화
        itemDetails = details;
        curLevel = 0;

        

        // 텍스트, 아이콘, 설명 등 셋팅


    }

    public void UpgradeItem()
    {
        // 레벨이 0일때
        if (item == null)
        {
            item = Instantiate(itemDetails.itemScript);
            item.Init(itemDetails);
        }

        // 아이템 업그레이드
        item.Upgrade();

        // 레벨 업
        curLevel++;
    }
}