using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float maxGameTime = 3 * 10f;
    public float gameTime;
    [Header("# Player Info")]
    public int level;
    public int kill;
    public int health;
    public int maxHeatlh = 30;
    public PoolManager pool;// 플레이어 프리팹을 인스펙터에서 할당
    public Player player; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당

    void Awake()
    {
        instance = this;
    }

    public void GameStart(){
        health = maxHeatlh;
        isLive = true;
    }
    void Update()
    {
        if(!isLive){
            return;
        }
        gameTime += Time.deltaTime;

        // 30초가 경과했을 때의 조건 검사
        if (gameTime >= 30f)
        {
            level++; 
            gameTime = 0f; 
        }
        
    }
    public void Stop(){
        isLive = false;
        Time.timeScale = 0; //원래는 1
    }
    public void Resume(){
        isLive = true;
        Time.timeScale = 1;
    }



}
