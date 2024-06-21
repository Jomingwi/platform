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
        mainCam = Camera.main; // ���� ī�޶�
        //ī�޶� 2���̻��ΰ��
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
        Vector2 fixedPos = mouseWorldPos - playerPos; //���콺�� �÷��̾�� ������


        // fixedPos.x > 0  or  transform.localScale.x  -1  = > right    ,   1  =>  left

        //Į�� ������ pivot �� �����
        //���ӿ�����Ʈ�� ���� �������ʿ� Ʈ�������� �����ؼ� Į�� �ڽ����� ����

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
