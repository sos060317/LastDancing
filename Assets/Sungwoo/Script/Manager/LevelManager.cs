using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform itemHandler;

    [SerializeField] private ItemCard itemCardPrefab;
    [SerializeField] private ItemDetails[] items;

    #region ΩÃ±€≈Ê

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
            itemCard.InitItemCard(item);
        }
    }

    public void ShowItemCard()
    {
        itemHandler.gameObject.SetActive(true);
    }

    public void HideItemCard()
    {
        itemHandler.gameObject.SetActive(false);
    }
}