using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//�巡�� ������ �κ��丮 ������

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler , IEndDragHandler
{
    Transform canvas; //�巡���Ҷ� UI�ڷ� �׷����°��� �����ϱ����� ��� �̿��� ĵ����
    Transform beforeParent; // Ȥ�� �߸��� ��ġ�� ����ϰ� �Ǹ� ���ƿ��ԵǴ� ��ġ

    CanvasGroup canvasGroup; //�ڽĵ��� ���� �����ϴ� ������Ʈ

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();    
    }

    void Start()
    {
        canvas = InventoryManager.Instance.CanvasInventory;   
    }

    /// <summary>
    /// idx�ѹ��� ���޹����� �ش� �������� Json���� ���� �˻��Ͽ� ã��
    /// �ش� ������ �����Ϳ��� �ʿ��� �������� �����ͼ� �ش� ��ũ��Ʈ�� ä����
    /// </summary>
    /// <param name="_idx"> �������� �ε��� �ѹ�</param>

    public void SetItem(string _idx)
    {

    }


    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        beforeParent = transform.parent;

        transform.SetParent(canvas);

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if(transform.parent == canvas)
        {
            transform.SetParent (beforeParent);
            transform.position = beforeParent.position;
        }

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}

    
