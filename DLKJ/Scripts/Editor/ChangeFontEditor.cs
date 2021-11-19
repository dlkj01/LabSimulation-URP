using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class ChangeFontEditor : MonoBehaviour
{
    [MenuItem("Tools/��������")]
    public static void GetFiles()//�ļ�������
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

