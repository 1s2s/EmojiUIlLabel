using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIEmojiLabel : UILabel
{
    private char emSpace = '\u0020';
    public List<GameObject> mEmojiList = new List<GameObject>();
	public GameObject emojiPrefab;
    List<EmojiStruct> mEmojiStructs = new List<EmojiStruct>();

	private float emSpaceCount;

	private int emojiWidth = 35;

    public UIAtlas emojiAtlas = null;

    private const string RegexSpaceStr = @"[\r\n]+";
    private const string RegexStr = @"@[0-9]{2}";

    private MatchCollection match;
    private MatchCollection mSpaceMatch;

    private StringBuilder spaceStr = new StringBuilder();

	struct EmojiStruct
    {
        public int index;
        public string emoji;

        public EmojiStruct(int i, string s)
        {
            this.index = i;
            this.emoji = s;
        }
    }

	protected override void Awake()
	{
		base.Awake();
		if (emojiPrefab != null&&emojiAtlas!= null)
        {
            UIEmojiWrapper.Instance.Init(emojiAtlas, emojiPrefab);
        }
        this.onPostFillCallBack = () =>
        {
            if(mEmojiStructs.Count>0)
            {
                mEmojiStructs.Clear();
            }
        };
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.text = "0123012@0112301231@0212\n大大实打实的大叔大@0323\n大叔大大叔大@01";
    }

    protected override void OnStart()
    {
        base.OnStart();
        //每一个空格的长度
        NGUIText.dynamicFont = trueTypeFont;
        float charLen = NGUIText.CalculatePrintedSize(emSpace.ToString()).x;
        emojiWidth = (int)(fontSize*1.5f);
        charLen = charLen == 0 ? 1 : charLen;
		//每一个表情的长度可以用多少个空格替代
		emSpaceCount = Mathf.CeilToInt(emojiWidth/charLen);
		for(var i = 0;i<emSpaceCount;i++)
		{
			spaceStr.Append(emSpace);
		}
    }


    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)//(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        base.OnFill(verts, uvs, cols);
       	StartCoroutine(GenerateEmojiText(text));
    }
	protected override void OnDisable() 
	{
		if(mEmojiList.Count>0)
		{
			UIEmojiWrapper.Instance.PushEmoji(mEmojiList);
			mEmojiList.Clear();
		}
		base.OnDisable();
	}
    /// <summary>
	/// 获取表情转移字符'@'顶点索引，并且需要排除空格换行符的部分，因为空格符UILabel是不会生成顶点的 所以需要减去空格符都数量，才能正确获得表情索引
	/// </summary>
	/// <returns></returns>
	private int GetIndex(int itemIndex)
    {
        Match item;
        int count = 0;
        for (int i = 0; i < mSpaceMatch.Count; i++)
        {
            item = mSpaceMatch[i];
            if (item.Index < itemIndex)
            {
                count++;
            }
        }
        return itemIndex - count;
    }

    public IEnumerator GenerateEmojiText(string inputString)
    {
		mEmojiStructs = new List<EmojiStruct>();
        string simpleStr = NGUIText.StripSymbols(inputString);
        match = Regex.Matches(simpleStr, RegexStr);
        mSpaceMatch = Regex.Matches(simpleStr, RegexSpaceStr);
        //记录每一个占位符的索引、对应的字段
        foreach (Match item in match)
		{
			EmojiStruct emoji = new EmojiStruct(item.Index,item.Value);
			if(!mEmojiStructs.Contains(emoji))
			{
				mEmojiStructs.Add(emoji);
			}
		}
        string emojiText = Regex.Replace(inputString, RegexStr, spaceStr.ToString());
        //Debug.Log("emojiTest" + emojiText);
        this.text = emojiText;
        yield return new WaitForFixedUpdate();
        //把之前用到表情重新放入对象池
        if (mEmojiStructs.Count > 0)
        {
            UIEmojiWrapper.Instance.PushEmoji(mEmojiList);
            mEmojiList.Clear();
        }
        for (var index = 0;index<mEmojiStructs.Count;index++)
		{
			GameObject go = UIEmojiWrapper.Instance.PopEmoji();
			if (go != null)
			{
				UISprite spr = go.GetComponent<UISprite>();
                //if (null == spr)
                //{
                //    spr = go.AddComponent<UISprite>();
                //}
                string emoji = UIEmojiWrapper.Instance.GetEmojiName(mEmojiStructs[index].emoji);
                if (!string.IsNullOrEmpty(emoji))
                {
                    spr.atlas = emojiAtlas;
                    spr.name = emoji;
                    spr.spriteName = emoji;
                    spr.width  = this.emojiWidth-3;
                    spr.height = this.emojiWidth-3;
                    spr.depth  = this.depth + 10;
                    try
                    {
                        spr.transform.parent = this.transform;
                        //int emojiIndex = mEmojiStructs[index].index * 4;
                        int start = GetIndex(mEmojiStructs[index].index) * 4;
                        int end = start + (mEmojiStructs[index].emoji.Length - 1) * 4 + 3;
                        //Debug.LogError("emojiIndex" + emojiIndex+" "+ "start" + start+" "+ "end" + end);
                        Vector3 space = Vector3.zero;
                        spr.transform.localPosition = new Vector3(this.geometry.verts[start].x + spr.width / 2,
                                                                 (this.geometry.verts[start+2].y)+ spr.height/ 2);
                        //Debug.Log("sprWidth" + spr.transform.localPosition);
                        spr.transform.localScale = Vector3.one;
                    }
                    catch
                    {
                        Debug.LogError("geometry.verts == null");
                    }
                }
                mEmojiList.Add(go);
            }
		}
	}
}

