using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using Common;
using System;
public enum UserType
{
    Null,
    Student,
    Teacher
}
public struct UserDataList
{
    public List<UserData> userDatas;
}
public struct UserData
{
    public UserType userType;
    public string accountNumber;
    public string password;
}
public class ExcelRead : MonoSingleton<ExcelRead>
{
    private string filePath = Application.streamingAssetsPath + "/账号表格.xlsx";
    List<UserData> userDataList = new List<UserData>();
    private void Awake()
    {
        StudentExcelData studentData = ExcelManager.GetInstance.GetExcelData<StudentExcelData, StudentExcelItem>();
        foreach (var item in studentData.items)
        {
            UserData userData = new UserData();
            userData.accountNumber = item.accountNumber;
            userData.password = item.password;
            string userType = item.userType.ToString();
            if (string.IsNullOrEmpty(userType))
                userType = "Null";
            Enum.TryParse(userType, out userData.userType);
            userDataList.Add(userData);

        }
        for (int i = 0; i < userDataList.Count; i++)
        {
            Debug.Log(userDataList[i].accountNumber);
        }
        UserData user = GetUserData("1");
        Debug.LogFormat("账号是{0}、密码是{1}、身份是{2}", user.accountNumber, user.password, user.userType.ToString());
    }

    public int Verify(string userName, string password)
    {
        if (HasUserName(userName) == false)
        {
            return 0;
        }
        if (PasswordIsRight(userName, password) == false)
        {
            return 1;
        }
        return 2;
    }

    private bool HasUserName(string userName)
    {
        for (int i = 0; i < userDataList.Count; i++)
        {
            if (userDataList[i].accountNumber == userName)
            {
                return true;
            }
        }
        return false;
    }

    private bool PasswordIsRight(string userName, string password)
    {
        return GetUserData(userName).password == password;
    }

    public UserData GetUserData(string id)
    {
        return userDataList.Find((P) => { return id == P.accountNumber; });
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
