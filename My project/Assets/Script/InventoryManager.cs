using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] GameObject viewInventory; // �κ��丮��
    [SerializeField] GameObject fabItem; // �κ��丮�� ������ ������

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
        Transform[] childs = viewInventory.GetComponentsInChildren<Transform>(); //�ڱ��ڽ��� �����ؼ� �˻��ϴ� ������ ����

        listTrsInventory.AddRange(childs);
        listTrsInventory.RemoveAt(0);

    }

    /// <summary>
    /// �κ��丮�� �����ִٸ� ���� 
    /// ���������� ����
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
    /// ����ִ� �κ��丮 �ѹ��� ���� , -1�� ���ϵȴٸ� ����ִ� ������ ���ٴ� �ǹ��Դϴ�.
    /// </summary>
    /// <returns> ����ִ� ������ ���� ��ȣ </returns>
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
