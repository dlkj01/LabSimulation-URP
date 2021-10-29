/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class StudentExcelItem : ExcelItemBase
{
	public string accountNumber;
	public string password;
	public string userType;
}

[CreateAssetMenu(fileName = "StudentExcelData", menuName = "Excel To ScriptableObject/Create StudentExcelData", order = 1)]
public class StudentExcelData : ExcelDataBase<StudentExcelItem>
{
}

#if UNITY_EDITOR
public class StudentAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		StudentExcelItem[] items = new StudentExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new StudentExcelItem();
			items[i].id = allItemValueRowList[i]["id"];
			items[i].accountNumber = allItemValueRowList[i]["accountNumber"];
			items[i].password = allItemValueRowList[i]["password"];
			items[i].userType = allItemValueRowList[i]["userType"];
		}
		StudentExcelData excelDataAsset = ScriptableObject.CreateInstance<StudentExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(StudentExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


