/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class Sheet1ExcelItem : ExcelItemBase
{
	public float X;
	public float Y;
	public float Distance;
}

[CreateAssetMenu(fileName = "Sheet1ExcelData", menuName = "Excel To ScriptableObject/Create Sheet1ExcelData", order = 1)]
public class Sheet1ExcelData : ExcelDataBase<Sheet1ExcelItem>
{
}

#if UNITY_EDITOR
public class Sheet1AssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		Sheet1ExcelItem[] items = new Sheet1ExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new Sheet1ExcelItem();
			items[i].id = allItemValueRowList[i]["id"];
			items[i].X = Convert.ToSingle(allItemValueRowList[i]["X"]);
			items[i].Y = Convert.ToSingle(allItemValueRowList[i]["Y"]);
			items[i].Distance = Convert.ToSingle(allItemValueRowList[i]["Distance"]);
		}
		Sheet1ExcelData excelDataAsset = ScriptableObject.CreateInstance<Sheet1ExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(Sheet1ExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


