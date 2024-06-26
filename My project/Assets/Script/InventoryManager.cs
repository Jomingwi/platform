using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] GameObject viewInventory; // 인벤토리뷰
    [SerializeField] GameObject fabItem; // 인벤토리에 생성될 프리팹

    [SerializeField] Transform canvasInventory;
    public Transform CanvasInventory => canvasInventory;

    List<Transform> listTrsInventory = new List<Transform>();


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
        initInventory();
        getEmptyItemSlot();
    }

    private void initInventory()
    {
        listTrsInventory.Clear();
        Transform[] childs = viewInventory.GetComponentsInChildren<Transform>(); //자기자신을 포함해서 검색하는 문제가 있음

        listTrsInventory.AddRange(childs);
        listTrsInventory.RemoveAt(0);

    }

    /// <summary>
    /// 인벤토리가 열려있다면 닫힘 
    /// 닫혀있으면 열림
    /// </summary>
    public void InActiveInventory()
    { 
        if(viewInventory.activeSelf == true)
        {
            viewInventory.SetActive(false);
        }
        else
        {
            viewInventory.SetActive(true);
        }

        //viewInventory.SetActive(!viewInventory.activeSelf);
    }
    /// <summary>
    /// 비어있는 인벤토리 넘버를 리턴 , -1이 리턴된다면 비어있는 슬롯이 없다는 의미입니다.
    /// </summary>
    /// <returns> 비어있는 아이템 슬롯 번호 </returns>
    private int getEmptyItemSlot()
    {
        int count = listTrsInventory.Count;
        for(int iNum = 0; iNum < count; ++iNum)
        {
            Transform trsSlot = listTrsInventory[iNum];
            if (trsSlot.childCount == 0 )
            {
                return iNum;
            }
        }
       return -1;
    }


}
