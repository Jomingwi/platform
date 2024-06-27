using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    InventoryManager inventoryManager;
    [SerializeField] string itemidx;

    private void Awake()
    {
        inventoryManager = InventoryManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) //���� ����� �÷��̾���
        {
            inventoryManager.GetItem(itemidx);
            //�κ��丮 �Ŵ������� ���� ���� �Ǵ��� Ȯ��
        }
    }
}
