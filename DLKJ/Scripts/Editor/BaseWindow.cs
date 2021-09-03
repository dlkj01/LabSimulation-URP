using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using static UnityEditor.Progress;
#if UNITY_EDITOR
using UnityEditor;
#endif

using DLKJ;
namespace DLKJ
{

    public class BaseWindow : EditorWindow {
		protected ItemDB itemDB;
        protected LabDB labDB;
		protected string[] abilitys = new string[500];
		protected string[] filters;

        public void OnReloadDBEvent()
        {
            LoadDB();
}

		public void LoadDB()
		{
            itemDB = ItemDB.LoadDB();
            labDB = LabDB.LoadDB();

        }

        protected string Localization(string key)
        {
            if (key == null)
            {
                return "Empty Key";
            }
            if (key == "")
            {
                return "Empty Key";
            }
            else
            {
                return "";
            }
        }

		protected Item DrawItemPopup(int itemID)
		{
			Item item;
			if (itemID < 0)
			{
				itemID = 0;
			}
			item = itemDB.GetItemByID(itemID);

			if (item == null)
			{
				itemID = item.ID;
			}

			int oldIndex = itemDB.GetItemIndexByID(item.ID);
			int index = 0;
			String[] names = itemDB.GetItemNames();
			index = EditorGUILayout.Popup("", oldIndex, names, new GUILayoutOption[] { GUILayout.Width(120) });
			if (index != oldIndex)
			{
				item = itemDB.GetItemByIndex(index);
			}

			return item;
		}

		protected int DrawPortPopup(int itemID, int portID)
		{
			int linkID = 0;
			Item connectItem = itemDB.GetItemByID(itemID);
            if (portID<0)
            {
				portID = connectItem.ports[0].ID;

            }

			linkID = portID;
			int index = 0;
			int oldIndex = connectItem.GetPortsIndex(portID);
			String[] names = connectItem.GetLinkPorts();
			index = EditorGUILayout.Popup("", oldIndex, names, new GUILayoutOption[] { GUILayout.Width(120) });
			if (index != oldIndex)
			{
				linkID = connectItem.GetPortByIndex(index).ID;
			}
			return linkID;
		}

	}
}