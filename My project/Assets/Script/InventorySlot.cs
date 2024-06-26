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
    /// 이벤트시스템으로 인해 드래그되는 대상이 이스트립트위에서 드롭되게되면
    /// 해당 드롭 오브젝트를 나의 자식으로 변경합니다
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
