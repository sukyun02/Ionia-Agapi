using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 25f;     // 좌우 이동 속도
    float jumpPower = 5f;     // 점프 힘
    float maxJumpTime = 0.15f; // 최대 점프 유지 시간 (초)
    float jumpTimeCounter;    // 점프 지속 시간 카운터

    bool isGrounded = false;  // 착지 여부 확인
    bool isJumping = false;   // 현재 점프 중인지 확인

    Rigidbody2D myRigid;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        myRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Jump();
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (isJumping) return;  // **점프 중에는 이동 애니메이션 적용 X**

        // 좌우 이동 애니메이션 처리
        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("isRun", true);
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        // X축 이동 적용, Y축은 현재 속도 유지 (점프 가능)
        Vector2 moveVector = new Vector2(moveHorizontal * moveSpeed, myRigid.linearVelocity.y);
        myRigid.linearVelocity = moveVector;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            jumpTimeCounter = maxJumpTime; // 점프 시간 초기화
            myRigid.linearVelocity = new Vector2(myRigid.linearVelocity.x, jumpPower);

            animator.SetBool("isJump", true);
            animator.SetBool("isRun", false);  // **점프하면 달리기 중지**
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                myRigid.linearVelocity = new Vector2(myRigid.linearVelocity.x, jumpPower);
                jumpTimeCounter -= Time.deltaTime; // 점프 지속 시간 감소
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;

            animator.SetBool("isJump", false);

            // **착지하면 달리기 애니메이션 다시 활성화**
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                animator.SetBool("isRun", true);
            }
        }
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class PlayerMovement : MonoBehaviour
// {
//     float moveSpeed = 25f;
//     
//     Rigidbody2D myRigid;
//     Animator animator;
//     SpriteRenderer spriteRenderer;
//
//     void Awake()
//     {
//         myRigid = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         spriteRenderer = GetComponent<SpriteRenderer>();
//     }
//
//     void Start()
//     {
//
//     }
//
//     private void FixedUpdate()
//     {
//         Move();
//     }
//
//     void Update()
//     {
//        
//     }
//
//     private void Move()
//     {
//         float moveHorizontal = Input.GetAxisRaw("Horizontal");
//         float moveVertical = Input.GetAxisRaw("Vertical");
//         if (moveHorizontal < 0)
//         {
//             spriteRenderer.flipX = true;
//             
//             animator.SetBool("isRun", true); 
//         }
//         else if (moveHorizontal > 0)
//         {
//             spriteRenderer.flipX = false;
//             animator.SetBool("isRun", true);
//         }
//         else if (moveVertical < 0 || moveVertical > 0)
//             animator.SetBool("isRun", true);
//         else if (moveHorizontal == 0 || moveVertical == 0)
//             animator.SetBool("isRun", false);
//
//         if (Input.GetButton("Jump"))
//         {
//             animator.SetBool("isJump", true);
//             Debug.Log("Space: isJump is true");
//         }
//         else if (Input.GetButtonUp("Jump"))
//         {
//             animator.SetBool("isJump", false);
//             Debug.Log("Space: isJump is false");
//         }
//         else
//         {
//             animator.SetBool("isJump", false);
//             Debug.Log("Space: else");
//         }
//
//         Vector2 moveVector = new Vector2(moveHorizontal, moveVertical);
//
//         myRigid.linearVelocity = moveVector * moveSpeed;
//     }
// }
