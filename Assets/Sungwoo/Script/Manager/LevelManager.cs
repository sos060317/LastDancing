using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform itemHandler;

    [SerializeField] private ItemCard itemCardPrefab;
    [SerializeField] private ItemDetails[] items;

    private List<ItemCard> itemList = new List<ItemCard>();
    private List<ItemCard> selectItemList = new List<ItemCard>();

    // 아이템들이 움직일 벡터 미리 캐싱
    private Vector3[] itemPos = { new Vector2(-580, -50), new Vector2(0, -50), new Vector2(580, -50) };
    private Vector3[] itemReadyPos = { new Vector2(-580, -1100), new Vector2(0, -1100), new Vector2(580, -1100) };

    System.Random random = new System.Random();

    #region 싱글톤

    private static LevelManager instance = null;

    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateItemCard();
    }

    private void CreateItemCard()
    {
        foreach (var item in items)
        {
            var itemCard = Instantiate(itemCardPrefab, itemHandler);
            itemCard.gameObject.SetActive(false);
            itemCard.InitItemCard(item);
            itemList.Add(itemCard);
        }
    }

    public void ShowItemCard()
    {
        itemHandler.gameObject.SetActive(true);

        // 아이템 선택
        selectItemList = itemList.Where(x => x.isMaxLevel == false)
                         .OrderBy(x => random.Next())
                         .Take(3)
                         .ToList();

        // 아이템 나오게 하기
        for(int i = 0; i < 3; i++)
        {
            selectItemList[i].gameObject.SetActive(true);
            selectItemList[i].rect.anchoredPosition = itemReadyPos[i];
            selectItemList[i].rect.DOAnchorPos(itemPos[i], 1f).SetEase(Ease.OutBack).SetDelay(0.3f * i);
        }
    }

    public void HideItemCard()
    {
        
    }

    private void HideItemRoutine()
    {
        // 아이템 숨기기
        for (int i = 0; i < 3; i++)
        {
            selectItemList[i].rect.DOAnchorPos(itemReadyPos[i], 1f).SetEase(Ease.InBack).SetDelay(0.3f * i)
                                  .OnComplete(() => selectItemList[i].gameObject.SetActive(false));
        }

        itemHandler.gameObject.SetActive(false);
    }
}