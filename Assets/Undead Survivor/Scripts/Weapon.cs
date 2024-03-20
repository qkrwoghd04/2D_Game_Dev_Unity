using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage = 2;
    public int count = 5;
    public float speed = 1000f;
    public float rotationDuration = 0.2f;

    private List<GameObject> spawnedBullets = new List<GameObject>(); // 생성된 칼들을 추적

    // public void Init(){
    //     switch(id){
    //         case 0:
    //             Batch();
    //             break;
    //         default:
    //             break;

    //     }
    // }

    void Start(){
        // Init();
    }
    
    // void Batch(){
    //     for(int i = 0; i < count; i++){
    //         Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
    //         bullet.parent = transform;

    //         bullet.localPosition = Vector3.zero; // 무기의 위치를 중심으로
    //         bullet.localRotation = Quaternion.Euler(Vector3.forward * 360 * i / count); // 로컬 회전 적용
    //         bullet.Translate(bullet.up * 1.5f, Space.World); 
    //         bullet.GetComponent<Bullet>().Init(damage, -1);
    //     }
    // }
    // Update is called once per frame
    void Update()
    {
        // switch(id){
        //     case 0:
        //         transform.Rotate(Vector3.back * speed * Time.deltaTime);
        //         break;
        //     default:
        //         break;

        // }
        if (Input.GetMouseButtonDown(1)) // 오른쪽 마우스 버튼 클릭 감지
        {
            prefabId = 2;
            StartCoroutine(SpawnAndRotateBullets());
        }
    }

    IEnumerator SpawnAndRotateBullets()
    {
        // 칼 생성 및 배치
        for (int i = 0; i < count; i++)
        {
            GameObject bullet = GameManager.instance.pool.Get(prefabId);
            Transform bulletTransform = bullet.transform;
            bulletTransform.parent = transform;
            bulletTransform.localPosition = Vector3.zero; // 무기의 위치를 중심으로
            bulletTransform.localRotation = Quaternion.Euler(Vector3.forward * 360 * i / count); // 로컬 회전 적용
            bulletTransform.Translate(bulletTransform.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1);
            spawnedBullets.Add(bullet); // 생성된 칼 추적
        }

        // 지정된 시간 동안 칼 회전
        float elapsedTime = 0;
        while (elapsedTime < rotationDuration)
        {
            transform.Rotate(Vector3.back * speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 회전 종료 후 칼 반환 및 리스트 클리어
        foreach (GameObject bullet in spawnedBullets)
        {
            bullet.transform.parent = null; // 부모 해제
            GameManager.instance.pool.Return(prefabId, bullet); // 풀에 반환
        }
        spawnedBullets.Clear();
    }

}
