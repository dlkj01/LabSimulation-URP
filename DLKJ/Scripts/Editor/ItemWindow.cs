using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

using DLKJ;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.Rendering;
using System.IO;

namespace DLKJ{
    public class ItemWindow : BaseWindow {

        private static ItemWindow window;
        protected Vector2 scrollPos;
        protected Vector2 buffScrollPos;
        private Vector2 listScroll;
        protected int selectIndex = 0;

		public static void Init() {
            window = (ItemWindow)EditorWindow.GetWindow(typeof(ItemWindow), false, "Item Editor");
            window.minSize = new Vector2(700, 500);
            window.LoadDB();
        }

		public void OnGUI (){
			if (window == null) {
				Init ();
			}

			GUILayout.Space (10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Add New One:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(120) });
			GameObject gameObject = (GameObject)EditorGUILayout.ObjectField(null, typeof(GameObject), false, new GUILayoutOption[] { GUILayout.Width(200) });
			if (gameObject)
			{
				Item item;
				if (!gameObject.TryGetComponent(out item))
                {
					AddNewItem(gameObject);
                }
                else
                {
					Debug.LogError("Contains Item!");
                }
			} 
			GUILayout.EndHorizontal();

			if (itemDB.items.Count == 0) return;

			GUILayout.BeginHorizontal();
			GUIItemList ();

            GUILayout.BeginVertical();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(560), GUILayout.Height(window.position.height - 120));
            DrawItemData ();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal ();

			if (GUI.changed) {
                for (int i = 0; i < itemDB.items.Count; i++)
                {
					EditorUtility.SetDirty(itemDB.items[i]);
				}
			}
		}

		string itemName = "";
		protected void GUIItemList() {
			GUILayout.BeginVertical();
			listScroll = EditorGUILayout.BeginScrollView(listScroll, GUILayout.Width(180), GUILayout.Height(window.position.height - 10));

			for (int i = 0; i < itemDB.items.Count; i++) {
				itemName = itemDB.items[i].itemName;
				GUILayout.BeginHorizontal();
				if (selectIndex == i)
				{
					GUI.color = Color.gray;
				}
				else
				{
					GUI.color = Color.white;
				}

				//Texture2D texture = null;
				//if (itemDB.items[i].icon)
				//{
				//	texture = itemDB.items[i].icon.texture;
				//}
				//else
				//{
				//	if(itemDB.items[i].gameObject) texture = AssetPreview.GetAssetPreview(itemDB.items[i].gameObject) as Texture2D;
				//}

				if (GUILayout.Button("-", new GUILayoutOption[] {GUILayout.Width (30), GUILayout.Height (30)
					}))
				{
					selectIndex = i;
					DeleteItem();
				}

                if (GUILayout.Button(itemName, new GUILayoutOption[] {
                        GUILayout.Width (150),
                        GUILayout.Height (30)
                    }))
                {
                    selectIndex = i;
                    //Texture2D texture = AssetPreview.GetAssetPreview(itemDB.items[i].gameObject) as Texture2D;
                    //string spriteName = itemName + ".png";
                    //string path = "Assets/DLKJ/Texture";

                    //SaveTextureToFile(texture, spriteName, path);
                    //AssetDatabase.Refresh();
                }

				GUILayout.EndHorizontal();
			}
			GUI.color = Color.white;

			GUILayout.EndScrollView ();
			GUILayout.EndVertical ();
			selectIndex = Mathf.Clamp(selectIndex, 0, itemDB.items.Count - 1);
		}

		void SaveTextureToFile(Texture2D texture, string fileName, string path)
		{
			var bytes = texture.EncodeToPNG();
			FileStream file = File.Open(path + "/" + fileName, FileMode.Create);
			var binary = new BinaryWriter(file);
			binary.Write(bytes);
			file.Close();
		}

		protected void DrawItemData(){
			Item selectItem = itemDB.GetItemByIndex(selectIndex);
			if (selectItem == null) {
				Debug.Log("NUll?"+selectIndex);
				return;
			}

			//Icon
			GUILayout.BeginHorizontal ();
			Texture texture = null;

			if (selectItem.icon) {

				texture = selectItem.icon;
            }
            else
            {
				texture = AssetPreview.GetAssetPreview(selectItem.gameObject) as Texture;
			}

			if (GUILayout.Button (texture, new GUILayoutOption[] {GUILayout.Width (100), GUILayout.Height (100)
			})) {
				
			}
				
			GUILayout.BeginVertical ();

			GUILayout.BeginHorizontal ();
			GUILayout.Label ("ID:" + selectItem.ID, EditorStyles.label, new GUILayoutOption[] { GUILayout.Width (120) });
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal();
			selectItem.itemName = EditorGUILayout.TextField(selectItem.name, new GUILayoutOption[] { GUILayout.Width(100) });
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Moveable:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });
			selectItem.moveable = EditorGUILayout.Toggle(selectItem.moveable, new GUILayoutOption[] { GUILayout.Width(70) });
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Library Type:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });
			selectItem.libraryType = (LibraryType)EditorGUILayout.EnumPopup(selectItem.libraryType, new GUILayoutOption[] { GUILayout.Width(100) });
			GUILayout.EndHorizontal();

            if (selectItem.libraryType == LibraryType.Wires)
            {
				GUILayout.BeginHorizontal();
				GUILayout.Label("Defult Pos:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });
				selectItem.portDefaultPosition = EditorGUILayout.Vector3Field("", selectItem.portDefaultPosition, new GUILayoutOption[] { GUILayout.Width(150) });
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("DefultRotate:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });
				selectItem.portDefaultEuler = EditorGUILayout.Vector3Field("", selectItem.portDefaultEuler, new GUILayoutOption[] { GUILayout.Width(150) });
				GUILayout.EndHorizontal();


			}

			GUILayout.BeginHorizontal();
            GUILayout.Label("RenderTexture:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
			RenderTexture renderTexture = (RenderTexture)EditorGUILayout.ObjectField(selectItem.renderTexture, typeof(RenderTexture), false, new GUILayoutOption[] { GUILayout.Width(120) });
            selectItem.renderTexture = renderTexture;
            GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Icon:", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(60) });
			selectItem.icon = (Texture2D)EditorGUILayout.ObjectField(selectItem.icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(120) });
			GUILayout.EndHorizontal();

			DrawConditions("Link Conditions:", selectItem.linkConditions);

			GUILayout.EndHorizontal();
		}

		void DrawConditions(string title, List<Condition> conditions)
		{
			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();

			GUILayout.Label(title, EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(title.Length * 7) });
			if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
			{
				Condition condition = new Condition();
				conditions.Add(condition);
			}
			GUILayout.EndHorizontal();

			for (int i = 0; i < conditions.Count; i++)
			{
				GUILayout.BeginHorizontal();
				conditions[i].data.itemID = DrawItemPopup(conditions[i].data.itemID).ID;
				Item item = itemDB.GetItemByID(conditions[i].data.itemID);
				if (item.ports.Count > 0) {
					conditions[i].data.portsID = DrawPortPopup(conditions[i].data.itemID, conditions[i].data.portsID);
					conditions[i].data.correct = EditorGUILayout.Toggle(conditions[i].data.correct, new GUILayoutOption[] { GUILayout.Width(20) });
				}
				

				if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(20), GUILayout.Height(20) }))
				{
					conditions.RemoveAt(i);
				}
				GUILayout.EndHorizontal();
			}

			GUILayout.EndVertical();
		}

		
		void AddNewItem(GameObject gameObject)
		{
			Item item = gameObject.AddComponent<Item>();
			item.ID = GenerateNewID();
			item.itemName = "ItemName" + item.ID;

            if (itemDB.items.Count==0)
            {
				itemDB.items.Add(item);
				selectIndex = 0;
            }
            else
            {
				itemDB.items.Insert(selectIndex+1,item);
				selectIndex = selectIndex + 1;
			}
		}

        void DeleteItem(){
			itemDB.items.RemoveAt(selectIndex);
			if (selectIndex >= itemDB.items.Count) {
				selectIndex = itemDB.items.Count - 1;
			}
		}

		protected int GenerateNewID(){
			int ID = 0;
			for (int i = 0; i < itemDB.items.Count; i++) {
				if (itemDB.items [i].ID > ID) {
					ID = itemDB.items [i].ID;
				}
			}
			ID += 1;
			return ID;
		}
			
	}
}
