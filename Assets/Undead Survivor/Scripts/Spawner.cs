using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    private bool called = false; 
    float meleeTimer = 0f;
    float rangedTimer = 0f;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Start()
    {
        Initialized();
    }
    public void Initialized(){
        // melee enemy 10마리 배치
        // 레벨 0에서 근접 적 10마리 배치
        if (GameManager.instance.level == 0)
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnMeleeEnemy();
            }
        }
    }
    void Update()
    {
        if (!GameManager.instance.isLive || GameManager.instance.score == 0) //score 가 0 == 죽은 enemy x
        {
            return;
        }
        
        if (GameManager.instance.level == 1 && !called)
        {
            Debug.Log("Called");
            GameManager.instance.pool.ResetAllPools();
            for (int i = 0; i < 10; i++)
            {
                SpawnMeleeEnemy();
            }
            for (int i = 0; i < 5; i++)
            {
                SpawnRangedEnemy();
            }
            called = true;
        }

        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;

        if (GameManager.instance.level == 0)
        {
            if (meleeTimer > 2f)
            {
                SpawnMeleeEnemy();
                meleeTimer = 0f; // 근접 적 타이머 재설정
            }
        }
        else if (GameManager.instance.level == 1)
        {
            if (meleeTimer > 2f)
            {
                SpawnMeleeEnemy();
                meleeTimer = 0f; // 근접 적 타이머 재설정
            }
            if (rangedTimer > 4f)
            {
                SpawnRangedEnemy();
                rangedTimer = 0f; // 원거리 적 타이머 재설정
            }
        }
    }

    void SpawnMeleeEnemy()
    {
        GameObject enemy = GameManager.instance.pool.Get(0); // 근접 적 생성
        enemy.GetComponent<Enemy>().enemyType = Enemy.EnemyType.Melee; // 적 유형 설정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

    void SpawnRangedEnemy()
    {
        GameObject enemy = GameManager.instance.pool.Get(1); // 원거리 적 생성
        enemy.GetComponent<Enemy>().enemyType = Enemy.EnemyType.Ranged; // 적 유형 설정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

}

