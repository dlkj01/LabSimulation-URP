using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using Common;

public enum UserType
{
    啥也不是,
    Student,
    Teacher
}
public class TextExcel : MonoSingleton<TextExcel>
{
    private string filePath = Application.streamingAssetsPath + "/账号表格.xlsx";
    public Dictionary<string, string> studentNameMap = new Dictionary<string, string>();
    public Dictionary<string, string> teacherNameMap = new Dictionary<string, string>();
    private void Awake()
    {
        AddUserData("Student", studentNameMap);
        AddUserData("Teacher", teacherNameMap);
        foreach (var item in studentNameMap)
        {
            Debug.Log(item.Key.ToString() + "===>" + item.Value.ToString());
        }
        foreach (var item in teacherNameMap)
        {
            Debug.Log(item.Key.ToString() + "===>" + item.Value.ToString());
        }
    }

    private void AddUserData(string sheetName, Dictionary<string, string> map)
    {
        List<string> numbers = GetColumnByName("账号", sheetName);
        List<string> passwords = GetColumnByName("密码", sheetName);
        for (int i = 0; i < numbers.Count; i++)
        {
            if (!map.ContainsKey(numbers[i]))
                map.Add(numbers[i], passwords[i]);
        }
    }

    public UserType GetUserType(string id)
    {
        if (studentNameMap.ContainsKey(id))
        {
            return UserType.Student;
        }
        if (teacherNameMap.ContainsKey(id))
        {
            return UserType.Teacher;
        }
        return UserType.啥也不是;
    }


    public List<string> GetColumnByName(string columnName, string sheetName)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        List<string> result = new List<string>();
        using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[sheetName];
            int column = 0;

            for (int i = 1; i < worksheet.Dimension.Columns + 1; i++)
            {
                if (worksheet.Cells[1, i].Value.ToString() == columnName)
                {
                    column = i;
                }
            }
            for (int j = 2; j < worksheet.Dimension.Rows + 1; j++)
            {
                if (worksheet.Cells[j, column].Value != null)
                {
                    result.Add(worksheet.Cells[j, column].Value.ToString());
                }
                else
                {
                    result.Add("weiotyweiobnsdhqiotryuqwiou");
                }
            }
        }
        return result;
    }
}
