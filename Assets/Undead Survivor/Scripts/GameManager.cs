using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int maxHeatlh;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당
    public GameObject uiResult;

    void Awake()
    {
        instance = this;
    }

    public void GameStart(){
        maxHeatlh = 30;
        health = maxHeatlh;
        isLive = true;
    }

    public void GameRetry(){
        SceneManager.LoadScene(0);
    }

    public void GameOver(){
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine(){
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.SetActive(true);
        Stop();
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
