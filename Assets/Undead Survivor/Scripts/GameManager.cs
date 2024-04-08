using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float maxGameTime;
    public float gameTime;
    [Header("# Player Info")]
    public int level = 0;
    public int score;
    public int health;
    public int maxHeatlh;

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당
    public Result uiResult;
    public GameObject enemyCleaner;
    public Spawner spawner;

    void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        maxHeatlh = 30;
        health = maxHeatlh;
        Resume();

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
    }
    public void AddScore(int points)
    {
        score += points;
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }
    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(1f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }
    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        if (level == 1)
        {
            Debug.Log("Level:" + level);
            enemyCleaner.SetActive(true);
            yield return new WaitForSeconds(2f);
            // 맵을 재배치합니다.
            health = maxHeatlh;
            FindObjectOfType<MapGenerator>().RegenerateMap();
            Resume();

            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        }
        else if (level == 2)
        {
            Debug.Log("Level:" + level);
            enemyCleaner.SetActive(true);
            yield return new WaitForSeconds(1f);
            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();
            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }
    }

    void Update()
    {
        if (!isLive)
        {
            return;
        }
        gameTime += Time.deltaTime;

        // 30초가 경과했을 때의 조건 검사
        if (gameTime >= 30f)
        {
            level++;
            gameTime = 0f;
            GameVictory();
        }

    }
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }



}
