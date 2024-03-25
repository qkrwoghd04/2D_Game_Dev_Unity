using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour //기본적으로 MonoBehaviour를 상속함
{
    public Vector2 inputVec;
    public float moveSpeed;
    public float damageCooldown = 1f; // 플레이어가 데미지를 입을 수 있는 최소 시간 간격입니다.
    private float lastDamageTime = -1f; // 마지막으로 데미지를 입은 시간을 추적합니다.
    Rigidbody2D rigid;
    SpriteRenderer sr;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.5f;
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        // fixedDeltaTime : 물리 프레임 하나가 소비한 시간
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            sr.flipX = inputVec.x < 0;
        }
    }

     void OnCollisionStay2D(Collision2D other)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        if (other.gameObject.CompareTag("Enemy") && Time.time - lastDamageTime > damageCooldown)
        {
            Debug.Log("Hit");
            GameManager.instance.health -= 1; // 매번 충돌 시 1의 데미지를 입습니다.
            lastDamageTime = Time.time; // 마지막 데미지 시간을 업데이트합니다.
            
            // Health가 0 이하가 되면 캐릭터 사망 처리
            if (GameManager.instance.health <= 0)
            {
                for (int i = 2; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                anim.SetTrigger("Dead");
                GameManager.instance.GameOver();
            }
        }
    }

}

