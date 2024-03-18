using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    float timer;
    int noe;

    void Awake(){
        spawnPoint = GetComponentsInChildren<Transform>();
    }
   void Update(){

        timer += Time.deltaTime;

        if(timer > 2f && noe <= 10){
            timer = 0;
            Spawn();
        }
   }

   void Spawn(){
        GameObject enemy = GameManager.instance.pool.Get(1);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        noe ++;
   }
}
