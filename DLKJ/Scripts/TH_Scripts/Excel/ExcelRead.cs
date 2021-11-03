using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OfficeOpenXml;
using System.IO;
using Common;
using System;
using DLKJ;

public enum UserType
{
    Null,
    Student,
    Teacher
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
}
