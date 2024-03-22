using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;
    Rigidbody2D rigid;

    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir){
        this.damage = damage;
        this.per = per;

        if(per > -1){
            rigid.velocity = dir * 5f;
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(per == -1){
            return;
        }
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            per --;
            gameObject.SetActive(false); // 총알을 비활성화
        }
    
        if(per == -1){
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
