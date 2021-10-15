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
    /// ���ݱ�ǩд������
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="CNName"></param>
    public static void HandleGuaranteeDoc(string fileName, Dictionary<string, string> map)
    {
        string filePath = streamingPath + "/" + fileName;
        string tempFile = Path.GetFullPath(filePath).ToString();      //��ȡģ��·����������ݸ���ģ��·��������
        Document doc = new Document(tempFile);
        DocumentBuilder builder = new DocumentBuilder(doc);   //����word
        foreach (var key in map.Keys)   //ѭ����ֵ��
        {
            builder.MoveToBookmark(key);  //�����������ǩ��λ��
            builder.StartBookmark(key).Bookmark.Text = map[key];
            // builder.Write(map[key]);   //���ֵ
        }
        doc.Save(filePath); //����word
    }

}
