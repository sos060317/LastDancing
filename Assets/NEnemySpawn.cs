using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NEnemySpawn : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public void Start()
    {
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            enemySpawner.NE(transform.position);
        }
    }
}
