using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class EnemySpawner : MonoBehaviourPun
{
    [SerializeField] private float spawnRange; //소환 범위
    [SerializeField] private float spawnTimer; //소환 시간
    [SerializeField] private float spawnDelay; //소환 시간
    [SerializeField] private float spawnCount; //소환 카운트

    private void Start()
    {
        spawnDelay = spawnTimer - 3.0f;
    }

    private void Update()
    {
        // 마스터 클라이언트만 적을 스폰
        if(PhotonNetwork.IsMasterClient)
        {
            spawnDelay += Time.deltaTime;

            if (spawnDelay > spawnTimer)
            {
                EnemySpawn();
                spawnDelay = 0;
            }
        }
    }

    /// <summary>
    /// 적 스폰 함수
    /// </summary>
    private void EnemySpawn()
    {
        spawnCount = Random.Range(1, 5);
        for(int i = 0; i < spawnCount; i++)
        {
            Vector3 randPos = Random.insideUnitSphere * spawnRange; //360도 구체안에서 랜덤좌표를 지정

            randPos.y = 1;

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Enemy Pistol"), randPos, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
