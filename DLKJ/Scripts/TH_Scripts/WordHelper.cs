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

            //if (item.FieldType.IsValueType || item.FieldType.Name.StartsWith("String"))
            //{
            if (value.ToString() == "-9999")
            {
                map.Add(name, "");
                continue;
            }
            map.Add(name, value);
            // }
        }
        return map;
    }
    private static string streamingPath = Application.streamingAssetsPath;
    static Document doc;
    /// <summary>
    /// 根据标签写入数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="CNName"></param>
    public static void HandleGuaranteeDoc(string fileName, Dictionary<string, object> map, string outPath)
    {
        string filePath = streamingPath + "/" + fileName;
        Stream stream = FileToStream(filePath);
        //     string tempFile = Path.GetFullPath(filePath).ToString();      //获取模板路径，这个根据个人模板路径而定。
        doc = new Document(stream/*tempFile*/);
        DocumentBuilder builder = new DocumentBuilder(doc);   //操作word
        foreach (var key in map.Keys)   //循环键值对
        {
            builder.MoveToBookmark(key);  //将光标移入书签的位置
            if (map[key] != null)
            {
                if (map[key] is AnswerCheck answerCheck)
                {
                    builder.StartBookmark(key).Bookmark.Text = answerCheck.answer.ToString();
                    //   builder.ParagraphFormat.Shading.BackgroundPatternColor = System.Drawing.Color.Yellow;
                    //Aspose.Words.Font font = builder.Font;
                    //font.Color = System.Drawing.Color.Yellow;
                    if (answerCheck.isRight == false)
                    {
                        Debug.LogError("wrong");

                    }
                    else
                    {
                        Debug.LogError("Yes:" + answerCheck.answer.ToString());
                    }
                }
                else
                {
                    builder.StartBookmark(key).Bookmark.Text = map[key].ToString();
                }

            }
        }
        doc.Save(streamingPath + "/Save/" + outPath); //保存word
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
}
