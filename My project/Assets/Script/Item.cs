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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) //닿은 대상이 플레이어라면
        {
            inventoryManager.GetItem(itemidx);
            //인벤토리 매니저에게 내가 습득 되는지 확인
        }
    }
}
