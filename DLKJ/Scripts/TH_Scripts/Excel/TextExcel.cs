using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
public class TextExcel : MonoBehaviour
{
    private string filePath = Application.streamingAssetsPath + "/账号表格.xlsx";
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveExcel();
        }
    }
    public void SaveExcel()
    {
        Debug.Log(filePath);
        FileInfo fileInfo = new FileInfo(filePath);
        Debug.Log(fileInfo.Name);
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            Debug.Log(excelPackage.Workbook.Names);
            //获取Excel的第一张表
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
            //获取第一行第一列的数据
            for (int i = 1; i < 4; i++)
            {
                Debug.Log(worksheet.Cells[i, 1].Value.ToString());
                Debug.Log(worksheet.Cells[i, 2].Value.ToString());
                Debug.Log(worksheet.Cells[i, 3].Value.ToString());
            }
            //////写入数据
            ////worksheet.Cells[1, 1].Value = "爸爸";
            //MainUiWindow mainUiWindow = FindObjectOfType<MainUiWindow>();
            ////从表格的第二行开始
            //for (int i = 2; i < mainUiWindow.ItemDic.Count + 2; i++)
            //{
            //    //第一列为编号
            //    worksheet.Cells[i, 1].Value = (i - 1).ToString();
            //    Debug.Log(worksheet.Cells[i, 1].Value = (i - 1).ToString());
            //    //第二列为队伍名称
            //    worksheet.Cells[i, 2].Value = mainUiWindow.ItemDic[i - 1].TeamID;
            //    //第三列为旗帜
            //    worksheet.Cells[i, 3].Value = mainUiWindow.ItemDic[i - 1].DuoQiID;
            //    //第四列为创建时间
            //    worksheet.Cells[i, 4].Value = mainUiWindow.ItemDic[i - 1].CreatTime;
            //}
            ////保存数据
            //excelPackage.Save();
        }
    }
}
