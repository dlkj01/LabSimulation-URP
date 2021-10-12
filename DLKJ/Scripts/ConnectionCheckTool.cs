using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 连接校验工具
/// </summary>
public static class ConnectionCheckTool
{
    private static Dictionary<ConnectionCheckType, List<string>> map;
    static ConnectionCheckTool()
    {
        map = new Dictionary<ConnectionCheckType, List<string>>();
        #region
        map.Add(ConnectionCheckType.三厘米测量线终端直接接短路版不接二端口网络时,
            new List<string>() { "微波信号源", "波导转同轴", "隔离器", "可变衰减器", "三厘米测量线" });
        #endregion
    }

}
public enum ConnectionCheckType
{
    三厘米测量线终端直接接短路版不接二端口网络时,
}