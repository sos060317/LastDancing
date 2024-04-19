using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetails_", menuName = "ScriptableObject/ItemDetails")]
public class ItemDetails : ScriptableObject
{
    #region 아이템 데이터

    [Space(10)]
    [Header("아이템 데이터")]
    public ItemData[] itemData;

    #endregion

    #region 아이템 기능 관련

    [Space(10)]
    [Header("아이템 기능 관련")]
    public ItemFunctionBase itemScript;

    #endregion

    [System.Serializable]
    public struct ItemData
    {
        public int itemCount;
        public float itemDamage;
    }
}