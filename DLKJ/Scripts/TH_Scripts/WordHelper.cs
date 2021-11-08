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
            string name = item.Name; //����
            object value = item.GetValue(t);  //ֵ
            if (value.ToString() == "-9999")
            {
                map.Add(name, "");
                continue;
            }
            map.Add(name, value);
        }
        return map;
    }
    private static string streamingPath = Application.streamingAssetsPath.Replace("StreamingAssets", "DocData");
    static Document doc;
    /// <summary>
    /// ���ݱ�ǩд������
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="CNName"></param>
    public static void HandleGuaranteeDoc(string fileName, Dictionary<string, object> map, string outPath)
    {
        string[] reportPath = new string[] { streamingPath + "/LabReport2.doc", streamingPath + "/LabReport3.doc", streamingPath + "/LabReport4.doc" };
        if (!Directory.Exists(streamingPath))
        {
            Directory.CreateDirectory(streamingPath);
        }
        string filePath = streamingPath + "/" + fileName;
        //for (int i = 0; i < reportPath.Length; i++)
        //{
        //    FileInfo info = new FileInfo(reportPath[i]);
        //    if (info.Exists)
        //        info.Attributes = FileAttributes.Hidden;
        //}

        Stream stream = FileToStream(filePath);
        //     string tempFile = Path.GetFullPath(filePath).ToString();      //��ȡģ��·����������ݸ���ģ��·��������
        doc = new Document(stream/*tempFile*/);
        DocumentBuilder builder = new DocumentBuilder(doc);   //����word
        foreach (var key in map.Keys)   //ѭ����ֵ��
        {
            builder.MoveToBookmark(key);  //�����������ǩ��λ��
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
            studentID = "ѧ������";
        doc.Save(savePath + "/" + studentID /*SceneManager.loginUserData.accountNumber*/ + "-" + DateTime.Now.ToString("yyyy-MM-dd") + outPath); //����word
        ProxyManager.saveProxy.Save();
        stream.Close();
    }

    public static Stream FileToStream(string fileName)
    {
        // ���ļ�
        FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        // ��ȡ�ļ��� byte[]
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();
        // �� byte[] ת���� Stream
        Stream stream = new MemoryStream(bytes);
        return stream;
    }
    public static LabReport2Data cacheData;
    public static UserDate cacheUserData;
}
