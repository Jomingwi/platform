using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveController : MonoBehaviour
{
    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    Animator anim;
    Vector3 moveDir;
    float vecticalVelocity = 0f; //수직으로 떨어지는 힘

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float grondCheckLength;//이길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알수가 없음
    [SerializeField] Color colorGroundCheck;
    [SerializeField] bool isGround; //인스펙터에서 플레이어가 플랫폼 타일에 착지 했는지 체크

    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, grondCheckLength), colorGroundCheck);
        }
         //Debug.DrawRay(); 디버그도 체킁요도로 씬카메라에 선을 그려 줄 수 있음
         //Gizmos.DrawSphere() 디버그 보다 더많은 시각 효과를 제공
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
        if (gameObject.CompareTag("Player") == true) //태그는 string으로 대상의 태그를 구분
        {

        }

        //Layer = int로 대상의 레이어를 구분
        //Layer의 int와 공통적으로 활용하는  int 와 다름
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
        //좌우키를 누르면 좌우로 움직인다
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed; // a LAKey -1 , d RAkey 1  아무것도 입력하지 않으면 0
        moveDir.y = rigid.velocity.y;
        //물리에의해 이동
        rigid.velocity = moveDir;
        
    }

}
