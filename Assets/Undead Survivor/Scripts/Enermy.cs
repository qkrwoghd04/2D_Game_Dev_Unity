using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy : MonoBehaviour
{
    public float speed = 2;
    public Rigidbody2D target;

    bool isLive = true;

    public float detectionDistance = 0.5f; // 장애물 감지 거리
    public LayerMask obstacleLayer; // 장애물 레이어

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    // Start is called before the first frame update
    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        if(!isLive) return;
        
        Vector2 directionToTarget = target.position - rigid.position;
        Vector2 moveDirection = AvoidObstacles(directionToTarget).normalized;

        Vector2 nextPosition = moveDirection * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPosition);
        rigid.velocity = Vector2.zero;
    }

    Vector2 AvoidObstacles(Vector2 directionToTarget) {
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, directionToTarget, detectionDistance, obstacleLayer);
        if (hit.collider != null) {
            // 장애물을 감지했을 경우, 장애물을 회피하는 방향으로 조정
            Vector2 avoidDirection = Vector2.Perpendicular(directionToTarget) * -1;
            return avoidDirection;
        }
        // 장애물이 없을 경우, 원래 타겟 방향으로 이동
        return directionToTarget;
    }

    void LateUpdate() {

        if(!isLive) return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable() {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }
}
