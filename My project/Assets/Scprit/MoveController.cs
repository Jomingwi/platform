using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        boxColl = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        camMain = Camera.main;
    }


    void Update()
    {
        checkGrounded();

        moving();
        checkAim();
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
        //RaycastHit2D hit = 
        //Physics2D.Raycast(transform.position, Vector2.down, grondCheckLength, LayerMask.GetMask("Ground"));


        RaycastHit2D hit =
        Physics2D.BoxCast(boxColl.bounds.center, boxColl.bounds.size, 0f, Vector2.down , grondCheckLength, LayerMask.GetMask("Ground"));

        if (hit)
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
