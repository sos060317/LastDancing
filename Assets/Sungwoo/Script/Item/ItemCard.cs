using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.IO;

public class ItemCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemDesc;

    [HideInInspector] public int curLevel;

    [HideInInspector] public bool isMaxLevel;

    [HideInInspector] public RectTransform rect;

    private ItemFunctionBase item;

    private ItemDetails itemDetails;

    public void InitItemCard(ItemDetails details)
    {
        // 변수 초기화
        itemDetails = details;
        curLevel = 0;
        rect = GetComponent<RectTransform>();

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
            item = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ItemPrefab", itemDetails.itemPathName), Vector3.zero, Quaternion.identity)
                .GetComponent<ItemFunctionBase>();
            item.Init(itemDetails);
        }

        // 만렙인지 체크
        if (curLevel >= itemDetails.itemData.Length - 1)
        {
            isMaxLevel = true;
        }

        // 아이템 업그레이드
        item.Upgrade();

        // 레벨 업
        curLevel = Mathf.Min(curLevel + 1, itemDetails.itemData.Length - 1);

        // 설명 초기화
        itemDesc.text = itemDetails.itemData[curLevel].itemDesc;

        LevelManager.Instance.HideItemCard();
        TimeManager.Instance.TimePlay();
    }
}