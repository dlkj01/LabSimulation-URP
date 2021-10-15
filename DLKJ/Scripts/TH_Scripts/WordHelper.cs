using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Aspose.Words;
using Aspose.Words.Tables;
using System.Linq;
public class WordHelper
{
    private static string streamingPath = Application.streamingAssetsPath;

    /// <summary>
    /// 根据标签写入数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="CNName"></param>
    public static void HandleGuaranteeDoc(string fileName, Dictionary<string, string> map)
    {
        string filePath = streamingPath + "/" + fileName;
        string tempFile = Path.GetFullPath(filePath).ToString();      //获取模板路径，这个根据个人模板路径而定。
        Document doc = new Document(tempFile);
        DocumentBuilder builder = new DocumentBuilder(doc);   //操作word
        foreach (var key in map.Keys)   //循环键值对
        {
            builder.MoveToBookmark(key);  //将光标移入书签的位置
            builder.StartBookmark(key).Bookmark.Text = map[key];
            // builder.Write(map[key]);   //填充值
        }
        doc.Save(filePath); //保存word
    }

}
