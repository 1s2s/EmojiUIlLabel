using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Text;


//主要把文本@01的文本给切分开来
//文本@01的用空格替换
//每一张图集的表情可以用空格来换算
//public class EmojiLabel : UILabel
//{
//    private char emSpace = '\u0020';
//    private char[] emChar = new char[3] { '@', '0', '1' };
//    private StringBuilder emSpaces;
//    private List<GameObject> emojiList = new List<GameObject>();
//    private List<emojiStruct> emojiStructs = new List<emojiStruct>();
//    public UIAtlas emojiAtlas = null;
//    表情长宽度
//    private const int emojiWidth = 16;
//    private int emSpaceNum = 0;

//    public struct emojiStruct
//    {
//        public int index;
//        public string emoji;
//        public emojiStruct(int index, string emoji)
//        {
//            this.index = index;
//            this.emoji = emoji;
//        }
//        public static bool operator ==(emojiStruct a, emojiStruct b)
//        {
//            return a.index == b.index;
//        }
//        public static bool operator !=(emojiStruct a, emojiStruct b)
//        {
//            return !(a.index == b.index);
//        }
//    }
//    protected override void OnStart()
//    {
//        UIEmojiWrapper.Instance.Init(emojiAtlas);
//        this.text = "010203121310@01123123@0231231@1大大";
//        base.OnStart();
//    }

//    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color> cols)
//    {
//        base.OnFill(verts, uvs, cols);
//        StartCoroutine(GenerateEmojiText(text));
//    }

//    public IEnumerator GenerateEmojiText(string str)
//    {
//        if (!string.IsNullOrEmpty(str))
//        {
//            float len = 10;//NGUIText.CalculatePrintedSize(emSpace.ToString()).x;
//            len = len == 0 ? 1 : len;
//            emSpaceNum = Mathf.CeilToInt(emojiWidth / len);
//            emSpaces = new StringBuilder();
//            for (var i = 0; i < emSpaceNum; i++)
//            {
//                emSpaces.Append(" ");
//            }
//            emojiStructs.Clear();
//            StringBuilder emojiText = new StringBuilder();
//            MatchCollection matches = Regex.Matches(str, @"([\s\S]*.)(@[0-9][0-9])([\s\S]*.)");
//            if (matches.Count > 0)
//            {



//                foreach (Match item in matches)
//                {
//                    if (item.Value.Length >= emChar.Length)
//                    {
//                        emojiStruct emoji = new emojiStruct(item.Index, item.Value.Substring(0, 3));
//                        if (!emojiStructs.Contains(emoji))
//                        {
//                            emojiStructs.Add(emoji);
//                        }
//                        Regex regex = new Regex(@"@[0-9][0-9]");
//                        string rStr = regex.Replace(item.Value, emSpaces.ToString(), 1);
//                        if (!string.IsNullOrEmpty(rStr))
//                        {
//                            emojiText.Append(rStr);
//                        }
//                    }
//                    else
//                    {
//                        emojiText.Append(item.Value);
//                    }
//                }
//                UIEmojiWrapper.Instance.PushEmoji(emojiList);
//                emojiList.Clear();
//                this.text = emojiText.ToString();
//                Debug.Log("this.text" + this.text);
//                yield return null;
//                for (var i = 0; i < emojiStructs.Count; i++)
//                {
//                    int index = emojiStructs[i].index;
//                    GameObject go = UIEmojiWrapper.Instance.PopEmoji();
//                    if (null != go)
//                    {
//                        UISprite spr = go.GetComponent<UISprite>();
//                        if (spr != null)
//                        {
//                            string emojiStr = UIEmojiWrapper.Instance.GetEmojiName(emojiStructs[i].emoji);
//                            if (!string.IsNullOrEmpty(emojiStr))
//                            {
//                                spr.name = string.Format("emoji_{0}", emojiStructs[i].index);
//                                spr.spriteName = emojiStr;
//                            }
//                            spr.width = emojiWidth;
//                            spr.height = emojiWidth;
//                            spr.transform.parent = this.transform;
//                            spr.transform.localScale = Vector3.one;
//                            spr.transform.localPosition = new Vector3(this.geometry.verts[index * 4].x + spr.width / 2,
//                                                                      this.geometry.verts[index * 4].y + spr.height / 2);
//                        }
//                        emojiList.Add(go);
//                    }
//                }
//            }
//            else
//            {
//                this.text = str;
//            }
//        }
//    }

//}
