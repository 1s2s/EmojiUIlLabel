#if !UNITY_3_5 && !UNITY_FLASH
#define DYNAMIC_FONT
#endif

using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UILabels.
/// </summary>

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UILabel))]
#else
[CustomEditor(typeof(UILabel), true)]
#endif
public class UIEmojiLabelInspector : UILabelInspector
{


    /// <summary>
    /// Draw the label's properties.
    /// </summary>

    protected override bool ShouldDrawProperties()
    {
        bool isValid = base.ShouldDrawProperties();

        EditorGUI.BeginDisabledGroup(!isValid);
        NGUIEditorTools.DrawProperty("emojiAtlas", serializedObject, "emojiAtlas");//表情所在的图集Atlas
        NGUIEditorTools.DrawProperty("emojiPrefab", serializedObject, "emojiPrefab");//表情所在的图集Atlas
        EditorGUI.EndDisabledGroup();
        return isValid;
    }
}
