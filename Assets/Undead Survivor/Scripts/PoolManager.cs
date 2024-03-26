using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리펩들을 보관할 변수
    public GameObject[] prefabs;
    // .. 풀 담당을 하는 리스트들 
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }

        Debug.Log(pools.Length);
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 비활성화 된 게임 오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        // ... 못 찾으면 새롭게 생성해서 select 변수에 할당

        return select;
    }
    public void ResetAllPools()
    {
        Debug.Log("Called");
        // 모든 풀을 순회합니다.
        foreach (List<GameObject> pool in pools)
        {
            // 해당 풀의 모든 오브젝트를 순회합니다.
            foreach (GameObject obj in pool)
            {
                Debug.Log("pool:"+ obj);
                // 오브젝트를 비활성화합니다.
                obj.SetActive(false);
            }
        }
    }

    public void Return(int prefabId, GameObject obj)
    {
        obj.SetActive(false);
    }

}
