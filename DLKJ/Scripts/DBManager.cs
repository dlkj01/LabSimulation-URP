using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DLKJ
{
    public class DBManager : MonoBehaviour
    {
        private static DBManager instance;
        private Dictionary<Type, object> DBs = new Dictionary<Type, object>();

        public static DBManager GetInstance()
        {
            if (null == instance)
            {
                instance = (DBManager)GameObject.FindObjectOfType(typeof(DBManager));
                instance.LoadDB();
            }
            return instance;
        }

        public void LoadDB()
        {
            if (!DBs.ContainsKey(typeof(ItemDB)))
            {
                DBs.Add(typeof(ItemDB), ItemDB.LoadDB());
            }

            if (!DBs.ContainsKey(typeof(LabDB)))
            {
                DBs.Add(typeof(LabDB), LabDB.LoadDB());
            }
        }

        public T GetDB<T>()
        {
            return (T)DBs[typeof(T)];
        }

        public void Clear()
        {
            DBs.Clear();
        }
    }
}