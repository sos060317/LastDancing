using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRange; //소환 범위
    [SerializeField] private float spawnTimer; //소환 시간
    [SerializeField] private float spawnDelay; //소환 시간
    [SerializeField] private float spawnCount; //소환 카운트
    [SerializeField] private EnemyBase enemyPrefab; //적 프리팹

    [SerializeField] private Transform target;

    private void Start()
    {
        spawnDelay = spawnTimer;
    }

    private void Update()
    {
        spawnDelay += Time.deltaTime;

        if(spawnDelay > spawnTimer )
        {
            EnemySpawn();
            spawnDelay = 0;
        }
    }

    private void EnemySpawn()
    {
        spawnCount = Random.Range(1, 5);
        for(int i = 0; i < spawnCount; i++)
        {
            Vector3 randPos = Random.insideUnitSphere * spawnRange; //360도 구체안에서 랜덤좌표를 지정

            randPos.y = 1;  

            var enemy = Instantiate(enemyPrefab, randPos, Quaternion.identity);
            enemy.target = GameManager.Instance.curPlayer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
