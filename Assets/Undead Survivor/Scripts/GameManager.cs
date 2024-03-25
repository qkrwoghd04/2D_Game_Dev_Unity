using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public float maxGameTime = 3 * 10f;
    public float gameTime;
    [Header("# Player Info")]
    public int level;
    public int kill;
    public PoolManager pool;// 플레이어 프리팹을 인스펙터에서 할당
    public Player player; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당

    void Awake() {
        instance = this;
    }
    void Update(){
        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime){
            gameTime = maxGameTime;
            level ++;
        }
    }
}
