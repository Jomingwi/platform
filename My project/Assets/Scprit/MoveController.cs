using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveController : MonoBehaviour
{
    [Header("�÷��̾� �̵� �� ����")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
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

    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, grondCheckLength), colorGroundCheck);
        }
         //Debug.DrawRay(); ����׵� üů�䵵�� ��ī�޶� ���� �׷� �� �� ����
         //Gizmos.DrawSphere() ����� ���� ������ �ð� ȿ���� ����
         //Handles.DrawWireArc
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {

    }


    void Update()
    {
        checkGrounded();

        moving();
        jump();

        checkGravity();

        doAnim();
    }

    private void checkGrounded()
    {
        isGround = false;

        if (verticalVelocity > 0f) return;

        //Layer = int�� ����� ���̾ ����
        //Layer�� int�� ���������� Ȱ���ϴ�  int �� �ٸ�
        //Wall Layer , Ground Layer
        RaycastHit2D hit = 
        Physics2D.Raycast(transform.position, Vector2.down, grondCheckLength, LayerMask.GetMask("Ground"));
        
        if(hit)
        {
            isGround = true;
        }
    }

    private void moving()
    {
        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed; // a LAKey -1 , d RAkey 1  �ƹ��͵� �Է����� ������ 0
        moveDir.y = rigid.velocity.y;
        //���������� �̵�
        rigid.velocity = moveDir;
        
    }

    private void jump()
    {
        //if (isGround == true  &&  Input.GetKeyDown(KeyCode.Space))
        //{
        //    rigid.AddForce(new Vector2(10, jumpForce), ForceMode2D.Impulse);//������ �̴� ��
        //}

        if (isGround == false)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
    }

    private void checkGravity()
    {
        if(isGround == false)//���߿� ������ ��
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // -9.81

            if(verticalVelocity < -10)
            {
                verticalVelocity = -10;
            }
        }
        else if(isJump == true)
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
