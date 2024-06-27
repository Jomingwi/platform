using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    public static JsonManager instance;

    List<cItemData> itemDatas;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        initJsonDatas();

    }

    private void initJsonDatas()
    {
        //itemData = (TextAsset)Resources.Load("ItemData");
        //itemData = Resources.Load<TextAsset>("ItemData"); // null
        TextAsset itemData = Resources.Load("ItemData") as TextAsset;
        itemDatas = JsonConvert.DeserializeObject<List<cItemData>>(itemData.ToString());
    }

    public string GetSpriteNameFromIdx(string _idx )
    {
        if(itemDatas == null) { return string.Empty; }
        return itemDatas.Find(x => x.idx == _idx).sprite;
    }
}
