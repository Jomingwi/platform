using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MoveController : MonoBehaviour
{
    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    BoxCollider2D boxColl;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0f; //수직으로 떨어지는 힘


    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float grondCheckLength;//이길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알수가 없음
    [SerializeField] Color colorGroundCheck;
    [SerializeField] bool isGround; //인스펙터에서 플레이어가 플랫폼 타일에 착지 했는지 체크
    bool isJump;

    Camera camMain;

    [Header("벽점프")]
    [SerializeField] bool touchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f;// 타이머

    [Header("대시")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashSpeed = 20f;
    float dashTimer = 0.0f;
    //대시이펙트

    [SerializeField] KeyCode dashKey;

    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, grondCheckLength), colorGroundCheck);
        }
        //Debug.DrawRay(); 디버그도 체크용도로 씬카메라에 선을 그려 줄 수 있음
        //Gizmos.DrawSphere() 디버그 보다 더많은 시각 효과를 제공
        //Handles.DrawWireArc
    }

    //private void OnTriggerEnter2D(Collider2D collision) //상대방의 콜라이더를 가져옴 , 누가실행시킨지는 모름
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        touchWall = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        touchWall = false;
    //    }
    //}

    public void TriggerEnter(HitBox.ehitboxType _type, Collider2D _collision)
    {
        if (_type == HitBox.ehitboxType.WallCheck)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = true;
            }
        }
    }

    public void TriggerExit(HitBox.ehitboxType _type, Collider2D _collision)
    {
        if (_type == HitBox.ehitboxType.WallCheck)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = false;
            }
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        camMain = Camera.main;
    }


    void Update()
    {
        checkTimer();
        checkGrounded();

        dash();

        moving();
        checkAim();
        jump();


        checkGravity();

        doAnim();
    }

    private void dash()
    {
        if(dashTimer == 0.0f && (Input.GetKeyDown(KeyCode.LeftShift)|| Input.GetKeyDown(KeyCode.F)))
        {
            dashTimer = dashTime;
            verticalVelocity = 0.0f;
            //if(transform.localScale.x > 0 )//왼쪽
            //{
            //    rigid.velocity = new Vector2(-dashSpeed, verticalVelocity);
            //}
            //else//오른쪽
            //{
            //    rigid.velocity = new Vector2(dashSpeed, verticalVelocity);
            //}
            //rigid.velocity = transform.localScale.x >0 
            //    ? new Vector2(-dashSpeed, verticalVelocity): new Vector2(dashSpeed, verticalVelocity);

            rigid.velocity = new Vector2(transform.localScale.x > 0 ? -dashSpeed : dashSpeed, 0);
        }
    }

    private void checkTimer()
    {
       if(wallJumpTimer > 0.0f)
        {
            wallJumpTimer -= Time.deltaTime;
            if(wallJumpTimer < 0.0f )
            {
                wallJumpTimer = 0.0f;
            }
        }

        if (dashTimer > 0.0f)
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer < 0.0f )
            {
                dashTimer = 0.0f;
            }
        }
    }

    private void checkGrounded()
    {
        isGround = false;

        if (verticalVelocity > 0f) return;

        //Layer = int로 대상의 레이어를 구분
        //Layer의 int와 공통적으로 활용하는  int 와 다름
        //Wall Layer , Ground Layer
        //RaycastHit2D hit = 
        //Physics2D.Raycast(transform.position, Vector2.down, grondCheckLength, LayerMask.GetMask("Ground"));


        RaycastHit2D hit =
        Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down, grondCheckLength, LayerMask.GetMask("Ground"));

        if (hit)
        {
            isGround = true;
        }
    }

    private void moving()
    {
        if(wallJumpTimer > 0.0f || dashTimer > 0.0f)
        {
            return;
        }

        //좌우키를 누르면 좌우로 움직인다
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed; // a LAKey -1 , d RAkey 1  아무것도 입력하지 않으면 0
        moveDir.y = rigid.velocity.y;
        //물리에의해 이동
        rigid.velocity = moveDir;
    }


    /// <summary>
    /// transform.localPosition; 부모로부터의 자식 포지션
    ///transform.position; 월드 포지션
    /// </summary>
    private void checkAim()
    {
        //Vector3 scale = transform.localScale;
        //if (moveDir.x < 0 && scale.x != 1.0f) //왼쪽
        //{
        //    scale.x = 1.0f;
        //    transform.localScale = scale;
        //}
        //else if (moveDir.x > 0 && scale.x != 1.0f) //오른쪽
        //{
        //    scale.x = -1.0f;
        //    transform.localScale = scale;
        //}

        Vector2 mouseWorldPos = camMain.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos;

        Vector3 playerScale = transform.localScale;
        if (fixedPos.x > 0 && playerScale.x != -1.0f)
        {
            playerScale.x = -1.0f;
        }
        else if (fixedPos.x < 0 && playerScale.x != 1.0f)
        {
            playerScale.x = 1.0f;
        }
        transform.localScale = playerScale;
    }


    private void jump()
    {
        //if (isGround == true  &&  Input.GetKeyDown(KeyCode.Space))
        //{
        //    rigid.AddForce(new Vector2(10, jumpForce), ForceMode2D.Impulse);//지긋이 미는 힘
        //}

        if (isGround == false) //공중에 떠있는 상태라면 , 벽에 붙어있고 , 벽을향해 플레이어가 방향키를 누르고 있는데 점프키를 누를때
        {
            if(touchWall == true && moveDir.x != 0 && Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
    }

    private void checkGravity()
    {
        if(dashTimer > 0)
        {
            return;
        }
        if(isWallJump  == true)
        {
            isWallJump = false;
            Vector2 dir = rigid.velocity;
            dir.x *= -1f; // 반대방향으로 전환
            rigid.velocity = dir;

            verticalVelocity = jumpForce * 0.5f;
            //일정시간 유저가 입력할수 없어야 벽을 발로찬 x값을 볼수잇음
            //입력불가 타이머를 작동시켜야함
            wallJumpTimer = wallJumpTime;

        }
        else if (isGround == false)//공중에 떠있을 때
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // -9.81

            if (verticalVelocity < -10)
            {
                verticalVelocity = -10;
            }
        }
        else if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }
        else if (isGround == true)
        {
            verticalVelocity = 0;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }
}
