using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace DLKJ
{
    public enum LibraryType
    {
        None,
        Equipment,  //设备库
        Device,     //器件库
        Wires,      //电线
    }

    public class ItemDB : MonoBehaviour
    {
        public List<Item> items = new List<Item>();

        public static ItemDB LoadDB()
        {
               GameObject obj = Resources.Load("ItemDB", typeof(GameObject)) as GameObject;

#if UNITY_EDITOR
            if (obj == null) obj = CreatePrefab();
#endif
            return obj.GetComponent<ItemDB>();
        }

        public Item GetItemByID(int ID)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == ID)
                {
                    return items[i];
                }
            }
            return items[0];
        }

        public Item GetItemByIndex(int index)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i == index)
                {
                    return items[i];
                }
            }
            return null;
        }

        public int GetItemIndexByID(int ID)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == ID)
                {
                    return i;
                }
            }
            return 0;
        }

        public String[] GetItemNames()
        {
            String[] names = new String[this.items.Count];
            for (int i = 0; i < this.items.Count; i++)
            {
                names[i] = this.items[i].itemName;
            }
            return names;
        }

        public List<Item> GetItemsByLibraryType(LibraryType type)
        {
            List<Item> libraryItems = new List<Item>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].libraryType == type)
                {
                    libraryItems.Add(items[i]);
                }
            }
            return libraryItems;
        }

#if UNITY_EDITOR
        private static GameObject CreatePrefab()
        {
            GameObject obj = new GameObject();
            obj.AddComponent<ItemDB>();
            GameObject prefab = PrefabUtility.CreatePrefab("Assets/DLKLJ/Resources/ItemDB.prefab", obj, ReplacePrefabOptions.ConnectToPrefab);
            DestroyImmediate(obj);
            AssetDatabase.Refresh();
            return prefab;
        }
#endif

    }
}