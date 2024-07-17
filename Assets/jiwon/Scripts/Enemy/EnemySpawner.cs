using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using System.IO;

public class EnemySpawner : MonoBehaviourPun
{
    [SerializeField] private float spawnRange; //소환 범위
    [SerializeField] private float spawnTimer; //소환 시간
    [SerializeField] private float spawnDelay; //소환 시간
    [SerializeField] private float spawnCount; //소환 카운트

    [SerializeField] private string[] enemyPrefabs;

    private bool isStop;

    private void Start()
    {
        enemyPrefabs = new string[]
        {
            Path.Combine("PhotonPrefabs", "Enemy", "Enemy Pistol"),
            Path.Combine("PhotonPrefabs", "Enemy", "Enemy Shotgun"),
        };

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

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randPos = Random.insideUnitSphere * spawnRange; // 360도 구체안에서 랜덤좌표를 지정
            string randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // 미리 파싱한 랜덤 적 프리팹 설정
            randPos.y = 0;

            NavMeshHit hit;

            if(NavMesh.SamplePosition(randPos, out hit, 10.0f, 1))
            {
                PhotonNetwork.Instantiate(randomEnemy, randPos, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
