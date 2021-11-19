using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class ChangeFontEditor : MonoBehaviour
{
    [MenuItem("Tools/更改字体")]
    public static void GetFiles()//文件夹名称
    {
        Font font = Resources.Load<Font>("SourceHanSansCN-Regular");
        GameObject[] objs = Selection.gameObjects;
        for (int i = 0; i < objs.Length; i++)
        {
            for (int j = 0; j < objs[i].GetComponentsInChildren<Text>(true).Length; j++)
            {
                objs[i].GetComponentsInChildren<Text>(true)[j].font = font;
                EditorUtility.SetDirty(objs[i]);
            }
        }
    }
}

