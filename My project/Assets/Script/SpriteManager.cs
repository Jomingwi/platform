using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    
    [SerializeField] List<Sprite> allSprites;

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
    }


    public Sprite GetSprite(string _spriteName)
    {
        int count = allSprites.Count;
        for(int iNum =0; iNum < count; ++iNum)
        {
            if(_spriteName == allSprites[iNum].name)
            {
                return allSprites[iNum];
            }
        }
        return null;
    }
}
