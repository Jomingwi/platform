using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MoveController : MonoBehaviour
{
    [Header("�÷��̾� �̵� �� ����")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    BoxCollider2D boxColl;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0f; //�������� �������� ��


    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float grondCheckLength;//�̱��̰� ���ӿ��� �󸶸�ŭ�� ���̷� �������� �������� ������������ �˼��� ����
    [SerializeField] Color colorGroundCheck;
    [SerializeField] bool isGround; //�ν����Ϳ��� �÷��̾ �÷��� Ÿ�Ͽ� ���� �ߴ��� üũ
    bool isJump;

    Camera camMain;

    [Header("������")]
    [SerializeField] bool touchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f;// Ÿ�̸�

    [Header("���")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashSpeed = 20f;
    float dashTimer = 0.0f;
    //�������Ʈ

    [SerializeField] KeyCode dashKey;

    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, grondCheckLength), colorGroundCheck);
        }
        //Debug.DrawRay(); ����׵� üũ�뵵�� ��ī�޶� ���� �׷� �� �� ����
        //Gizmos.DrawSphere() ����� ���� ������ �ð� ȿ���� ����
        //Handles.DrawWireArc
    }

    //private void OnTriggerEnter2D(Collider2D collision) //������ �ݶ��̴��� ������ , ���������Ų���� ��
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
            //if(transform.localScale.x > 0 )//����
            //{
            //    rigid.velocity = new Vector2(-dashSpeed, verticalVelocity);
            //}
            //else//������
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

        //Layer = int�� ����� ���̾ ����
        //Layer�� int�� ���������� Ȱ���ϴ�  int �� �ٸ�
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

        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed; // a LAKey -1 , d RAkey 1  �ƹ��͵� �Է����� ������ 0
        moveDir.y = rigid.velocity.y;
        //���������� �̵�
        rigid.velocity = moveDir;
    }


    /// <summary>
    /// transform.localPosition; �θ�κ����� �ڽ� ������
    ///transform.position; ���� ������
    /// </summary>
    private void checkAim()
    {
        //Vector3 scale = transform.localScale;
        //if (moveDir.x < 0 && scale.x != 1.0f) //����
        //{
        //    scale.x = 1.0f;
        //    transform.localScale = scale;
        //}
        //else if (moveDir.x > 0 && scale.x != 1.0f) //������
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
        //    rigid.AddForce(new Vector2(10, jumpForce), ForceMode2D.Impulse);//������ �̴� ��
        //}

        if (isGround == false) //���߿� ���ִ� ���¶�� , ���� �پ��ְ� , �������� �÷��̾ ����Ű�� ������ �ִµ� ����Ű�� ������
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
            dir.x *= -1f; // �ݴ�������� ��ȯ
            rigid.velocity = dir;

            verticalVelocity = jumpForce * 0.5f;
            //�����ð� ������ �Է��Ҽ� ����� ���� �߷��� x���� ��������
            //�ԷºҰ� Ÿ�̸Ӹ� �۵����Ѿ���
            wallJumpTimer = wallJumpTime;

        }
        else if (isGround == false)//���߿� ������ ��
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
