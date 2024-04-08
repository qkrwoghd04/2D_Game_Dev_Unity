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
    public Text[] rankTexts; // Inspector에서 할당

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
        spawner.ResetSpawner();
        player.ResetPosition();
        Resume();
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

        pool.ResetAllPools();
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
            gameRanking.SetActive(true);
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

    void ShowRanking()
    {
        // 랭킹 정보를 불러와서 저장
        int[] bestScores = new int[5];
        for (int i = 0; i < 5; i++)
        {
            bestScores[i] = PlayerPrefs.GetInt("BestScore" + i, 0);
        }

        // 현재 점수가 베스트 스코어에 들어갈 자리를 찾아 업데이트
        for (int i = 4; i >= 0; i--)
        {
            if (score > bestScores[i])
            {
                if (i < 4)
                {
                    bestScores[i + 1] = bestScores[i];
                }
                bestScores[i] = score;
            }
            else
            {
                break;
            }
        }

        // 업데이트된 베스트 스코어를 다시 저장
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("BestScore" + i, bestScores[i]);
            // UI에 베스트 스코어 표시 업데이트
            rankTexts[i].text = (i + 1) + ". " + bestScores[i];
        }
    }
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        // 에디터에서 실행 중이라면 에디터 플레이 모드 종료
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
