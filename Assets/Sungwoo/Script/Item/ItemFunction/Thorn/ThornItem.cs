using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ThornItem : ItemFunctionBase
{
    [SerializeField] private GameObject thornPrefab;

    Vector3 dir;
    Vector3 startPos;

    private int curLevel = -1;

    private bool isAttacking = false;

    private Transform player;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
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

        if (!pv.IsMine)
        {
            yield break;
        }

        while (true)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = transform.position.y;
            dir.z = Random.Range(-1f, 1f);

            startPos = transform.position;

            for (int i = 0; i < 7; i++)
            {
                //Instantiate(thornPrefab, dir.normalized * (i + 1) + startPos, Quaternion.identity);
                var thorn = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ItemPrefab", "Thorn"),
                    player.position + (dir.normalized * (i + 1) + startPos), Quaternion.identity).GetComponent<Thorn>();

                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }

            yield return YieldInstructionCache.WaitForSeconds(3);
        }
    }
}