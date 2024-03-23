using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    // public SpawnData[] spawnData;
    int level = 1;
    float meleeTimer = 0f;
    float rangedTimer = 0f;

    void Awake(){
        spawnPoint = GetComponentsInChildren<Transform>();
    }
   void Update(){

        level = Mathf.FloorToInt(GameManager.instance.gameTime / 30f) + 1;

        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;

        if(level == 1){
            if(meleeTimer > 2f){
                SpawnMeleeEnemy();
                meleeTimer = 0f; // 근접 적 타이머 재설정
            }
        } else if(level == 2){
            if(meleeTimer > 2f){
                SpawnMeleeEnemy();
                meleeTimer = 0f; // 근접 적 타이머 재설정
            }
            if(rangedTimer > 4f){
                SpawnRangedEnemy();
                rangedTimer = 0f; // 원거리 적 타이머 재설정
            }
        }
   }

   void SpawnMeleeEnemy(){
        GameObject enemy = GameManager.instance.pool.Get(0); // 근접 적 생성
        enemy.GetComponent<Enemy>().enemyType = Enemy.EnemyType.Melee; // 적 유형 설정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

    void SpawnRangedEnemy(){
        GameObject enemy = GameManager.instance.pool.Get(1); // 원거리 적 생성
        enemy.GetComponent<Enemy>().enemyType = Enemy.EnemyType.Ranged; // 적 유형 설정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}

