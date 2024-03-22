using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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
    private float meleeAttackCooldown = 2.0f;
    private float rangedAttackCooldown = 0.5f;
    private float lastMeleeAttackTime = -2.0f;
    private float lastRangedAttackTime = -0.5f;
    void Update()
    {
       if (Input.GetMouseButtonDown(1) && Time.time - lastMeleeAttackTime >= meleeAttackCooldown) // 오른쪽 마우스 버튼 클릭 감지 및 쿨다운 확인
        {
            lastMeleeAttackTime = Time.time; // 마지막 근접 공격 시간 업데이트
            prefabId = 2;
            StartCoroutine(SpawnAndRotateBullets());
        }
        if (Input.GetMouseButtonDown(0) && Time.time - lastRangedAttackTime >= rangedAttackCooldown) // 왼쪽 마우스 버튼 클릭 감지 및 쿨다운 확인
        {
            lastRangedAttackTime = Time.time; // 마지막 원거리 공격 시간 업데이트
            prefabId = 3;
            FireRangedAttack();
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
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
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
            bullet.transform.parent = null; 
            GameManager.instance.pool.Return(prefabId, bullet); // 풀에 반환
        }
        spawnedBullets.Clear();
    }

    void FireRangedAttack()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = transform.position.z; // 추가
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        Debug.Log("Dir=" + dir);
        bullet.GetComponent<Bullet>().Init(1, 0, dir);
    }

}
