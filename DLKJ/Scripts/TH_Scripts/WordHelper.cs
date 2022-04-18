using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Aspose.Words;
using System.Linq;
using System.Reflection;
using DLKJ;
using Common;

public static class WordHelper
{

    public static Dictionary<string, object> GetFields<T>(T t)
    {
        float a = 0;
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
            if (value.ToString() == "-9999" || value.ToString() == "-99999999")
            {
                map.Add(name, "");
                continue;
            }
            if (value is double d)
            {
                if (d.ToString().Contains('.'))
                {
                    string[] result = d.ToString().Split('.');
                    if (result.Length <= 1)
                    {
                        map.Add(name, d.ToString("#0"));
                        continue;
                    }
                    if (result[1].Length == 1)
                    {
                        map.Add(name, d.ToString("#0.0")); continue;
                    }
                    if (result[1].Length == 2)
                    {
                        map.Add(name, d.ToString("#0.00")); continue;
                    }
                    if (result[1].Length == 3)
                    {
                        map.Add(name, d.ToString("#0.000")); continue;
                    }
                    if (result[1].Length == 4)
                    {
                        map.Add(name, d.ToString("#0.0000")); continue;
                    }
                    if (result[1].Length >= 5)
                    {
                        map.Add(name, d.ToString("#0.00000")); continue;
                    }
                }
            }
            map.Add(name, value);
        }
        return map;
    }
    private static string streamingPath = Application.streamingAssetsPath + "/DocTemplate";
    public static Dictionary<string, string> resultMap = new Dictionary<string, string>();
    /// <summary>
    /// 根据标签写入数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="CNName"></param>
    public static Document HandleGuaranteeDoc(string fileName, Dictionary<string, object> map, string outPath)
    {

#if UNITY_WEBGL
        foreach (var key in map.Keys)   //循环键值对
        {
            if (map[key] != null)
            {
                if (map[key] is AnswerCheck answerCheck)
                {
                    string result = string.Empty;
                    if (answerCheck.isRight == false)
                        result = answerCheck.answer.ToString() + "(Wrong)";
                    else
                        result = answerCheck.answer.ToString();
                    resultMap.Add(key, result);
                }
                else
                    resultMap.Add(key, map[key].ToString());
            }
        }
#else
        if (!Directory.Exists(streamingPath))
        {
            Directory.CreateDirectory(streamingPath);
        }
        string filePath = streamingPath + "/" + fileName;
        //Stream stream = FileToStream(filePath);
        Document doc = new Document(filePath);
        DocumentBuilder builder = new DocumentBuilder(doc);   //操作word
        foreach (var key in map.Keys)   //循环键值对
        {
            builder.MoveToBookmark(key);  //将光标移入书签的位置
            //builder.Font.Color = System.Drawing.Color.Black;
            //builder.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.White;
            if (map[key] != null)
            {
                if (map[key] is AnswerCheck answerCheck)
                {
                    string result = string.Empty;
                    //  builder.Font.Color = System.Drawing.Color.Green;
                    if (answerCheck.isRight == false)
                        result = answerCheck.answer.ToString() + "(Wrong)";
                    else
                        result = answerCheck.answer.ToString();
                    builder.StartBookmark(key).Bookmark.Text = result;
                }
                else
                {
                    builder.StartBookmark(key).Bookmark.Text = map[key].ToString();
                }
            }
        }
        //写入分数
        builder.MoveToBookmark("Score");
        builder.StartBookmark("Score").Bookmark.Text = ProxyManager.saveProxy.GetScoreBySceneAfterConversion().ToString("#0.00");
        Debug.Log("当前实验得分："+ProxyManager.saveProxy.GetScoreBySceneAfterConversion().ToString());
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
        //stream.Close();
        return doc;
#endif
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
