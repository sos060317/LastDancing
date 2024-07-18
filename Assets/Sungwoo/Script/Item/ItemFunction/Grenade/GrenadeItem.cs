using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class GrenadeItem : ItemFunctionBase
{
    [SerializeField] private Grenade grenadePrefab;

    private int curLevel = -1;

    private bool isAttacking;

    private Transform player;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

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
        player = GameManager.Instance.curPlayer;

        while (true)
        {
            for (int i = 0; i < itemDetails.itemData[curLevel].itemCount; i++)
            {
                if (!GameManager.Instance.curPlayer.gameObject.activeSelf)
                    continue;

                GenerationGrenade();
            }

            yield return YieldInstructionCache.WaitForSeconds(6f);
        }
    }

    private void GenerationGrenade()
    {
        var grenade = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ItemPrefab", "Grenade"),
                player.position + Vector3.up * 2, Quaternion.identity).GetComponent<Grenade>();

        grenade.Init(3, itemDetails.itemData[curLevel].itemDamage);
    }
}