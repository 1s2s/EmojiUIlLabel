#if !UNITY_3_5 && !UNITY_FLASH
#define DYNAMIC_FONT
#endif

using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UILabels.
/// </summary>

[CanEditMultipleObjects]
[CustomEditor(typeof(UILabel), true)]
public class EmojiLabelInspector : UILabelInspector
{

    /// <summary>
    /// Draw the label's properties.
    /// </summary>
    protected override bool ShouldDrawProperties()
    {
        bool isValid = base.ShouldDrawProperties();

        EditorGUI.BeginDisabledGroup(!isValid);
        NGUIEditorTools.DrawProperty("Atlas", serializedObject, "emojiAtlas");//表情所在的图集Atlas
        EditorGUI.EndDisabledGroup();
        return isValid;
    }
}
