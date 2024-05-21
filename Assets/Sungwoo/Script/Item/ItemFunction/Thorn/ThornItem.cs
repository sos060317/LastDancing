using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornItem : ItemFunctionBase
{
    [SerializeField] private GameObject thornPrefab;

    Vector3 dir;
    Vector3 startPos;

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

    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            dir.x = Random.Range(-1f, 1f);
            dir.y = transform.position.y;
            dir.z = Random.Range(-1f, 1f);

            startPos = transform.position;

            for (int i = 0; i < 7; i++)
            {
                Instantiate(thornPrefab, dir.normalized * (i + 1) + startPos, Quaternion.identity);
                yield return YieldInstructionCache.WaitForSeconds(0.1f);
            }

            yield return YieldInstructionCache.WaitForSeconds(3);
        }
    }
}