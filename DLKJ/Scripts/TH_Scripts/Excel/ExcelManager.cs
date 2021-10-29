using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Common;
public class ExcelManager : Singleton<ExcelManager>
{
    Dictionary<Type, object> excelDataDic = new Dictionary<Type, object>();
    /// <summary>
    /// 获取表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <returns></returns>
    public T GetExcelData<T, V>() where T : ExcelDataBase<V> where V : ExcelItemBase
    {
        Type type = typeof(T);
        if (excelDataDic.ContainsKey(type) && excelDataDic[type] is T)
            return excelDataDic[type] as T;

        T excelData = Resources.Load<T>(ConstConfig.excelAssetResourcesPath + type.Name);
        if (excelData != null)
            excelDataDic.Add(type, excelData as T);

        return excelData;
    }
    /// <summary>
    /// 获取单项数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="targetId"></param>
    /// <returns></returns>
    public V GetExcelItem<T, V>(string targetId) where T : ExcelDataBase<V> where V : ExcelItemBase
    {
        var excelData = GetExcelData<T, V>();
        if (excelData != null)
            return excelData.GetExcelItem(targetId);
        return null;
    }
}
