using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playerPrefab; // 플레이어 프리팹을 인스펙터에서 할당
    public Transform mapCenterTransform; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당

    // 게임 시작 시 호출되는 Start 함수
    void Start()
    {
        SpawnPlayerAtMapCenter();
    }

    // 플레이어를 맵의 중심에 생성하는 함수
    void SpawnPlayerAtMapCenter()
    {
        if (playerPrefab != null && mapCenterTransform != null)
        {
            // 플레이어 프리팹을 맵의 중심 위치에 인스턴스화
            Instantiate(playerPrefab, mapCenterTransform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerPrefab 또는 MapCenterTransform이 할당되지 않았습니다.");
        }
    }
}
