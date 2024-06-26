using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour 
    ,IPointerEnterHandler , IPointerExitHandler , IDropHandler
{
    Image imgSlot;
    RectTransform rectTransform;

    private void Awake()
    {
        imgSlot = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        imgSlot.color = Color.red;

    }
    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        imgSlot.color = Color.white;
    }
   

    /// <summary>
    /// �̺�Ʈ�ý������� ���� �巡�׵Ǵ� ����� �̽�Ʈ��Ʈ������ ��ӵǰԵǸ�
    /// �ش� ��� ������Ʈ�� ���� �ڽ����� �����մϴ�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.position = rectTransform.position;

            //}
        }
    }

}
