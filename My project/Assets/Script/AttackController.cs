using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Transform trsHand;
    [SerializeField] GameObject objThrowWeapon;
    [SerializeField] Transform trsWeapon;
    
    private void Start()
    {
        mainCam = Camera.main; // 메인 카메라
        //카메라가 2개이상인경우
        //Camera.current

    }
    void Update()
    {
        checkAim();
    }

    private void checkAim()
    {
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos; //마우스와 플레이어간의 오차값


        // fixedPos.x > 0  or  transform.localScale.x  -1  = > right    ,   1  =>  left

        //칼을 돌릴때 pivot 값 변경과
        //게임오브젝트를 만들어서 손잡이쪽에 트랜스폼을 지정해서 칼을 자식으로 넣음

        float angle = Quaternion.FromToRotation(
           transform.localScale.x > 0 ? Vector3.left : Vector3.right, fixedPos).eulerAngles.z;
        trsHand.rotation = Quaternion.Euler(0, 0, angle);


    }

    private void checkCreate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            createWeapon();
        }
    }

    private void createWeapon()
    {
        //GameObject go = Instantiate(objThrowWeapon , trsHand);
    }
}
