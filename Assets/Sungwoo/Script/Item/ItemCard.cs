using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemDesc;

    [HideInInspector] public int curLevel;

    private ItemFunctionBase item;

    public ItemDetails test;

    private ItemDetails itemDetails;

    private void Start()
    {
        InitItemCard(test);
    }

    public void InitItemCard(ItemDetails details)
    {
        // 변수 초기화
        itemDetails = details;
        curLevel = 0;

        // 텍스트, 아이콘, 설명 셋팅
        itemName.text = itemDetails.itemName;
        itemImage.sprite = itemDetails.itemImage;
        itemDesc.text = itemDetails.itemData[curLevel].itemDesc;
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
        curLevel = Mathf.Min(curLevel + 1, itemDetails.itemData.Length - 1);

        // 설명 초기화
        itemDesc.text = itemDetails.itemData[curLevel].itemDesc;
    }
}