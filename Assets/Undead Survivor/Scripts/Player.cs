using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour //기본적으로 MonoBehaviour를 상속함
{
    public Vector2 inputVec;
    public float moveSpeed;
    Rigidbody2D rigid;
    SpriteRenderer sr;
    Animator anim;

    void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.5f;
    }

    void FixedUpdate() {
        // fixedDeltaTime : 물리 프레임 하나가 소비한 시간
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }
    void OnMove(InputValue value){
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate() {
        anim.SetFloat("Speed", inputVec.magnitude);

        if(inputVec.x != 0){
            sr.flipX = inputVec.x < 0;
        }
    }
}

