using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager pool;// 플레이어 프리팹을 인스펙터에서 할당
    public Player player; // 맵 중심 위치를 나타내는 Transform, 인스펙터에서 할당

    void Awake() {
        instance = this;
    }
}
