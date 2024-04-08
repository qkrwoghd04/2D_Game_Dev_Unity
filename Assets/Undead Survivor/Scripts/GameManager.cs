using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float maxGameTime;
    public float gameTime;
    public Text rankScoreCurrent; // Inspector에서 할당  
    public GameObject gameRanking;

    [Header("# Player Info")]
    public int level = 0;
    public int score;
    public int health;
    public int maxHeatlh = 30;
    private int[] bestScore = new int[5];

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당
    public Result uiResult;
    public GameObject enemyCleaner;
    public Spawner spawner;
    public GameObject pauseMenuUI; // Inspector에서 할당

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
        ResetGame();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        health = maxHeatlh;
        isLive = true;
        score = 0;
        level = 0;
        gameTime = 0f;

        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
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
        ShowRanking(); // 랭킹 표시 추가
        Stop();
        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    public void ResetGame()
    {
        // 게임 상태 초기화
        isLive = true;
        score = 0;
        level = 0;
        health = maxHeatlh;
        gameTime = 0f;

        // UI 초기화
        uiResult.gameObject.SetActive(false); // 결과 패널 비활성화
        gameRanking.SetActive(false); // 랭킹 패널 비활성화

        pool.ResetAllPools(); // 오브젝트 풀 초기화 (PoolManager 스크립트에 해당 메서드 구현 필요)
        

        // 기타 게임 컴포넌트 초기화
        // 예: 타이머 재설정, 점수판 초기화 등

        // 배경음 및 효과음 재생
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
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
            ShowRanking(); // 랭킹 표시 추가
            Stop();
            AudioManager.instance.PlayBgm(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
        }
    }

    void Update()
    {
        // 스페이스바가 눌렸을 때의 처리
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 게임이 진행 중이라면 일시 정지
            if (isLive)
            {
                Pause();
            }
        }
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

    public void Pause()
    {
        isLive = false;
        Time.timeScale = 0; // 게임 시간을 멈춤
        pauseMenuUI.SetActive(true);
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
    public void LoadHome()
    {
        SceneManager.LoadScene(0); // "HomeSceneName"을 실제 메인 메뉴 씬 이름으로 변경하세요.
        Time.timeScale = 1; // 게임 시간을 재개
    }


    void ScoreSet(int currentScore)
    {
        PlayerPrefs.SetInt("CurrentPlayerScore", currentScore);

        int tmpScore = 0;

        for (int i = 0; i < 5; i++)
        {
            bestScore[i] = PlayerPrefs.GetInt(i + "BestScore");

            while (bestScore[i] < currentScore)
            {
                tmpScore = bestScore[i];
                bestScore[i] = currentScore;

                PlayerPrefs.SetInt(i + "BestScore", currentScore);

                currentScore = tmpScore;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(i + "BestScore", bestScore[i]);
        }
    }

    public void ShowRanking()
    {

        int currentPlayerScore = score; // 현재 플레이어의 점수
        ScoreSet(currentPlayerScore); // 점수 저장 및 랭킹 갱신

        // UI 텍스트에 현재 점수와 베스트 점수 표시
        string rankingText = "Current Score: " + currentPlayerScore + "\nBest Scores:\n";
        for (int i = 0; i < 5; i++)
        {
            int bestScore = PlayerPrefs.GetInt(i + "BestScore", 0);
            rankingText += (i + 1) + ": " + bestScore + "\n";
        }
        gameRanking.SetActive(true);
        rankScoreCurrent.text = string.Format("Score. {0:F0}", score);
        Debug.Log(rankingText);
    }
}
