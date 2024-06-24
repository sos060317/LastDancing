using System.Collections;
using UnityEngine;
using Photon.Pun;

public class GrenadeItem : ItemFunctionBase
{
    [SerializeField] private Grenade grenadePrefab;

    private int curLevel = -1;

    private bool isAttacking;

    private Transform player;
    private PhotonView pv;

    public override void Init(ItemDetails details)
    {
        this.itemDetails = details;
        pv = GetComponent<PhotonView>();
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
            pv.RPC(nameof(GenerationGrenade), RpcTarget.All);

            yield return YieldInstructionCache.WaitForSeconds(6f);
        }
    }

    [PunRPC]
    private void GenerationGrenade()
    {
        for (int i = 0; i < itemDetails.itemData[curLevel].itemCount; i++)
        {
            var grenade = Instantiate(grenadePrefab, player.position + Vector3.up * 2, Quaternion.identity);

            grenade.Init(3);
        }
    }
}