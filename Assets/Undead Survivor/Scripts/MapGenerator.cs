using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap destructibleOtc; // 파괴 가능한 장애물을 위한 Tilemap
    public Tilemap indestructibleOtc; // 파괴 불가능한 장애물을 위한 Tilemap

    public Tile destructibleTile; // 파괴 가능한 장애물 타일
    public Tile indestructibleTile; // 파괴 불가능한 장애물 타일

    private int width = 22; // 장애물이 생성될 가로 범위
    private int height = 22; // 장애물이 생성될 세로 범위
    public int destructibleObstacles = 15; // 생성할 파괴 가능한 장애물의 수
    public int indestructibleObstacles = 15; // 생성할 파괴 불가능한 장애물의 수

    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

    void Start()
    {
        GenerateMap();
        
    }
    

    void GenerateMap()
    {
        // 초기 장애물 위치 셋을 초기화
        occupiedPositions.Clear();

        // 장애물 배치
        GenerateObstacles(destructibleOtc, destructibleTile, destructibleObstacles);
        GenerateObstacles(indestructibleOtc, indestructibleTile, indestructibleObstacles);
    }

    void GenerateObstacles(Tilemap tilemap, Tile tile, int obstacleCount)
    {
        for (int i = 0; i < obstacleCount; i++)
        {
            int attempt = 0; // 무한 루프 방지를 위한 시도 횟수
            bool placed = false;
            while (!placed && attempt < 100) // 장애물을 배치할 수 있을 때까지 시도
            {
                int x = Random.Range(1 - width / 2, width / 2);
                int y = Random.Range(1 - height / 2, height / 2);
                Vector3Int position = new Vector3Int(x, y, 0);

                if (!occupiedPositions.Contains(position))
                {
                    tilemap.SetTile(position, tile);
                    occupiedPositions.Add(position);
                    placed = true;
                }
                attempt++;
            }
        }
    }
}

