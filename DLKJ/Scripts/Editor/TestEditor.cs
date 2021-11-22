using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TestEditor
{
    [MenuItem("Tools/更改矩形长宽 L")]
    static void SetRectTF()
    {
        Transform[] gos = Selection.transforms;
        foreach (var item in gos)
        {
            RectTransform tf = item as RectTransform;
            for (int i = 0; i < tf.GetComponentsInChildren<RectTransform>().Length; i++)
            {
                tf.GetComponentsInChildren<RectTransform>()[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tf.rect.width);
                tf.GetComponentsInChildren<RectTransform>()[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tf.rect.height);
            }
        }

    }
}
