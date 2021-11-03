/*Auto Create, Don't Edit !!!*/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public class ExperimentInputVerifyExcelItem : ExcelItemBase
{
	public string ExperimentalType;
	public int ExperimentalStep;
	public string[] InputTextName;
}

[CreateAssetMenu(fileName = "ExperimentInputVerifyExcelData", menuName = "Excel To ScriptableObject/Create ExperimentInputVerifyExcelData", order = 1)]
public class ExperimentInputVerifyExcelData : ExcelDataBase<ExperimentInputVerifyExcelItem>
{
}

#if UNITY_EDITOR
public class ExperimentInputVerifyAssetAssignment
{
	public static bool CreateAsset(List<Dictionary<string, string>> allItemValueRowList, string excelAssetPath)
	{
		if (allItemValueRowList == null || allItemValueRowList.Count == 0)
			return false;
		int rowCount = allItemValueRowList.Count;
		ExperimentInputVerifyExcelItem[] items = new ExperimentInputVerifyExcelItem[rowCount];
		for (int i = 0; i < items.Length; i++)
		{
			items[i] = new ExperimentInputVerifyExcelItem();
			items[i].id = allItemValueRowList[i]["id"];
			items[i].ExperimentalType = allItemValueRowList[i]["ExperimentalType"];
			items[i].ExperimentalStep = Convert.ToInt32(allItemValueRowList[i]["ExperimentalStep"]);
			items[i].InputTextName = new string[]{ "SourceFrequency","Attenuator","EquivalentSectionPosition" };
		}
		ExperimentInputVerifyExcelData excelDataAsset = ScriptableObject.CreateInstance<ExperimentInputVerifyExcelData>();
		excelDataAsset.items = items;
		if (!Directory.Exists(excelAssetPath))
			Directory.CreateDirectory(excelAssetPath);
		string pullPath = excelAssetPath + "/" + typeof(ExperimentInputVerifyExcelData).Name + ".asset";
		UnityEditor.AssetDatabase.DeleteAsset(pullPath);
		UnityEditor.AssetDatabase.CreateAsset(excelDataAsset, pullPath);
		UnityEditor.AssetDatabase.Refresh();
		return true;
	}
}
#endif


