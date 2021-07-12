using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEmojiWrapper
{
    private GameObject emojiPrefab;
    private UIAtlas emojiAltas;
    private bool isInit = false;
    //表情的转义对应着表情的名称
    private Dictionary<string,string> emojiName = new Dictionary<string, string>();

    private List<GameObject> useSpr = new List<GameObject>();
    private Queue<GameObject> freeSpr = new Queue<GameObject>();

    private Vector3 OutOffScreen = new Vector3(10000f, 10000f, 10000f);

    private static UIEmojiWrapper sInstance;
    public static UIEmojiWrapper Instance
    {
        get
        {
            if(null == sInstance)
            {
                sInstance = new UIEmojiWrapper();
            }
            return sInstance;
        }
    }

    public void Init(UIAtlas atlas)
    {
        if(!isInit)
        {
            //做加载图集
            //创建首个预制体
            if(null == emojiAltas)
            {
                emojiAltas = atlas;
            }
            for (int i = 0, cnt = emojiAltas.spriteList.Count; i < cnt; i++)
            {
                emojiName.Add(emojiAltas.spriteList[i].name, emojiAltas.spriteList[i].name);
            }
            emojiPrefab = new GameObject();
            UISprite spr = emojiPrefab.AddComponent<UISprite>();
            if(null!=spr)
            {
                spr.transform.localPosition = OutOffScreen;
                spr.name = "emoji";
                spr.atlas = emojiAltas;
                spr.onPostFill += OnPostFill;
            }
            freeSpr.Enqueue(emojiPrefab);
            isInit = true;
        }
    }
    public bool HasEmoji(string key)
    {
        return emojiName!=null&&emojiName.ContainsKey(key);
    }

    /// <summary>
    /// 未用的表情不销毁
    /// </summary>
    /// <param name="emoji"></param>
    public void PushEmoji(GameObject emoji)
    {
        if(null == emoji) return; 
        emoji.transform.localPosition = OutOffScreen;
        emoji.transform.parent = null;
        emoji.gameObject.SetActive(false);
        freeSpr.Enqueue(emoji);
    } 

    public void PushEmoji(List<GameObject> emojiList)
    {
        for(var i = 0;i<emojiList.Count;i++)
        {
            PushEmoji(emojiList[i]);
        }
    }

    public GameObject PopEmoji()
    {
        if(freeSpr.Count==0)
        {
            if(emojiPrefab == null)
                 return null;
            GameObject emoji = GameObject.Instantiate(emojiPrefab);
            UISprite emojiSpr = emoji.GetComponent<UISprite>();
            if(emojiSpr!=null)
            {
                emojiSpr.hideIfOffScreen = true;
                emojiSpr.atlas = emojiAltas;
                emojiSpr.onPostFill += OnPostFill;
            }
            freeSpr.Enqueue(emoji);
        }
        GameObject go = freeSpr.Dequeue();
        
        if (go != null)
        {
            useSpr.Add(go);
            go.gameObject.SetActive(true);
        }
        return go;
    }

    public string GetEmojiName(string encode)
    {
        string retSpr = string.Empty;
        if(emojiName!=null)
        {
            emojiName.TryGetValue(encode,out retSpr);
        }
        return retSpr;
    }

    public void OnPostFill (UIWidget widget, int bufferOffset, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
     {
         if (widget != null)
         {
             if(!widget.isVisible)
             {
                 UISprite spt = widget as UISprite;
                 if (spt != null)
                 {
                     PushEmoji(spt.gameObject);
                 }
             }
         }
     }
}
