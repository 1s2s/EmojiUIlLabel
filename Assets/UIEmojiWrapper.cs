using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEmojiWrapper
{
    private GameObject emojiPrefab;
    private UIAtlas emojiAltas;
    private UIRoot _root;
    public UIRoot Root
    {
        get
        {
            if (null == _root)
            {
                _root = GameObject.FindObjectOfType<UIRoot>();
            }
            return _root;
        }
    }

    private Transform _freeSprPoolTrans;
    public Transform FreeSprPool
    {
        get
        {
            if(null == _freeSprPoolTrans)
            {
                GameObject go = GameObject.Find("UIRoot/FreeEmojiPool");
                if (go == null)
                {
                    go = new GameObject();
                }
                go.name = "FreeEmojiPool";
                go.transform.SetParent(Root.transform);
                go.transform.localPosition = Vector3.one;
                go.transform.localScale = Vector3.one;
                _freeSprPoolTrans = go.transform;
            }
            return _freeSprPoolTrans;
        }
    }


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

    public void Init(UIAtlas atlas,GameObject emojiprefab)
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
            if(null == emojiPrefab)
            {
                this.emojiPrefab = emojiprefab;
            }
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
        if(useSpr.Contains(emoji))
        {
            useSpr.Remove(emoji);
            emoji.transform.parent = null;
            emoji.transform.localPosition = OutOffScreen;
            emoji.gameObject.SetActive(false);
            emoji.transform.parent = FreeSprPool;
            freeSpr.Enqueue(emoji);
        }
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
                emojiSpr.transform.localPosition = OutOffScreen;
                emojiSpr.name = "emoji";
                emojiSpr.atlas = emojiAltas;
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
   
}
