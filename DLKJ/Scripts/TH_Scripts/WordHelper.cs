using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Linq;
using System.Reflection;
using DLKJ;
using Common;

public static class WordHelper
{

    public static Dictionary<string, object> GetFields<T>(T t)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();
        System.Reflection.FieldInfo[] fields = t.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (fields.Length <= 0)
        {
            return map;
        }
        foreach (System.Reflection.FieldInfo item in fields)
        {
            string name = item.Name; //名称
            object value = item.GetValue(t);  //值
            if (value.ToString() == "-9999")
            {
                map.Add(name, "");
                continue;
            }
            map.Add(name, value);
        }
        return map;
    }
    private static string streamingPath = Application.streamingAssetsPath + "/DocTemplate";
    static Document doc;
    /// <summary>
    /// 根据标签写入数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="CNName"></param>
    public static void HandleGuaranteeDoc(string fileName, Dictionary<string, object> map, string outPath)
    {
        if (!Directory.Exists(streamingPath))
        {
            Directory.CreateDirectory(streamingPath);
        }
        string filePath = streamingPath + "/" + fileName;
        Stream stream = FileToStream(filePath);
        doc = new Document(stream/*tempFile*/);
        DocumentBuilder builder = new DocumentBuilder(doc);   //操作word
        foreach (var key in map.Keys)   //循环键值对
        {
            builder.MoveToBookmark(key);  //将光标移入书签的位置
            if (map[key] != null)
            {
                if (map[key] is AnswerCheck answerCheck)
                {
                    string result;
                    if (answerCheck.isRight == false)
                    {
                        result = answerCheck.answer.ToString() + "(Wrong)";
                    }
                    else
                    {
                        result = answerCheck.answer.ToString();
                    }
                    builder.StartBookmark(key).Bookmark.Text = result;
                }
                else
                {
                    builder.StartBookmark(key).Bookmark.Text = map[key].ToString();
                }
            }
        }
        string savePath = Application.streamingAssetsPath + "/Save";
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        string studentID = UIManager.GetInstance().UILabButton.uiLabReport.GetInputValue("IDInputField");
        if (string.IsNullOrEmpty(studentID))
            studentID = "学号有误";
        doc.Save(savePath + "/" + studentID /*SceneManager.loginUserData.accountNumber*/ + "-" + DateTime.Now.ToString("yyyy-MM-dd") + outPath); //保存word
        ProxyManager.saveProxy.Save();
        stream.Close();
    }

    public static Stream FileToStream(string fileName)
    {
        // 打开文件
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        // 读取文件的 byte[]
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();
        // 把 byte[] 转换成 Stream
        Stream stream = new MemoryStream(bytes);
        return stream;
    }
    public static LabReport2Data cacheData;
    public static UserDate cacheUserData;
}
