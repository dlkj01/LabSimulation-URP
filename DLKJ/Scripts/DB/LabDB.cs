using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DLKJ
{
    public class LabDB : MonoBehaviour
{
    public List<Lab> labs = new List<Lab>();

    public static LabDB LoadDB()
    {
        GameObject obj = Resources.Load("LabDB", typeof(GameObject)) as GameObject;

#if UNITY_EDITOR
        if (obj == null) obj = CreatePrefab();
#endif
        return obj.GetComponent<LabDB>();
    }

    public Lab GetLabByID(int ID)
    {
        for (int i = 0; i < labs.Count; i++)
        {
            if (labs[i].ID == ID)
            {
                return labs[i];
            }
        }
        return null;
    }

    public Lab GetLabByIndex(int index)
    {
        for (int i = 0; i < labs.Count; i++)
        {
            if (i == index)
            {
                return labs[i];
            }
        }
        return null;
    }

    public int GetLabIndexByID(int ID)
    {
        for (int i = 0; i < labs.Count; i++)
        {
            if (labs[i].ID == ID)
            {
                return i;
            }
        }
        return 0;
    }

    public String[] GetLabNames()
    {
        String[] names = new String[this.labs.Count];
        for (int i = 0; i < this.labs.Count; i++)
        {
            names[i] = this.labs[i].labName;
        }
        return names;
    }

#if UNITY_EDITOR
        [Obsolete("该方法已过期，请使用新方法")]
        private static GameObject CreatePrefab()
    {
        GameObject obj = new GameObject();
        obj.AddComponent<LabDB>();
        GameObject prefab = PrefabUtility.CreatePrefab("Assets/DLKLJ/Resources/LabDB.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
        DestroyImmediate(obj);
        AssetDatabase.Refresh();
        return prefab;
    }
#endif

}
}