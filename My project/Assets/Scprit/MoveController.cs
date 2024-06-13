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
    float vecticalVelocity = 0f; //�������� �������� ��

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float grondCheckLength;//�̱��̰� ���ӿ��� �󸶸�ŭ�� ���̷� �������� �������� ������������ �˼��� ����
    [SerializeField] Color colorGroundCheck;
    [SerializeField] bool isGround; //�ν����Ϳ��� �÷��̾ �÷��� Ÿ�Ͽ� ���� �ߴ��� üũ

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
    }

    void Start()
    {

    }


    void Update()
    {
        checkGrounded();
        moving();
    }

    private void checkGrounded()
    {
        if (gameObject.CompareTag("Player") == true) //�±״� string���� ����� �±׸� ����
        {

        }

        //Layer = int�� ����� ���̾ ����
        //Layer�� int�� ���������� Ȱ���ϴ�  int �� �ٸ�
        //Wall Layer , Ground Layer
        RaycastHit2D hit = 
        Physics2D.Raycast(transform.position, Vector2.down, grondCheckLength, LayerMask.GetMask("Ground"));
        
        if(hit)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
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

}
