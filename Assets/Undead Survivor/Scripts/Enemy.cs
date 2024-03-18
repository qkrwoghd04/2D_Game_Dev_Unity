using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2;
    public float health;
    public float Max_health = 1;
    // public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive = true;
    public float detectionDistance = 0.5f; // 장애물 감지 거리
    public LayerMask obstacleLayer; // 장애물 레이어
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    // Animator anim;

    // public void InitMelee(SpawnData data){
    //     anim.runtimeAnimatorController = animCon[data.spriteType];
    //     speed = data.speed;
    //     Melee_Max_health = data.health + 1;
    //     melee_health = data.health + 1;
    // }
    // public void InitRanged(SpawnData data){
    //     anim.runtimeAnimatorController = animCon[data.spriteType];
    //     speed = data.speed;
    //     Ranged_Max_health = data.health;
    //     Ranged_health = data.health;
    // }

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        // anim = GetComponent<Animator>();
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
        isLive = true;
        health = Max_health;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(!collision.CompareTag("Bullet")) return;

        health -= collision.GetComponent<Bullet>().damage;

        if(health > 0){

        }else{
            Dead();
        }
    }

    void Dead(){
        gameObject.SetActive(false);
    }
}