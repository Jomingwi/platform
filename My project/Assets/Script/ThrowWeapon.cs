using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 force;
    bool right;
    [SerializeField] bool isDone = false;



    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        rigid.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isDone= true;
    }


    void Update()
    {
        if (isDone == true) return;

        transform.Rotate(new Vector3(0,0, right == true ? -360f : 360f) * Time.deltaTime);
    }

    public void SetForce(Vector2 _force , bool _isRight)
    {
        force = _force;
        right = _isRight;

    }
}
