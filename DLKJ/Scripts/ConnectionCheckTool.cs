using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����У�鹤��
/// </summary>
public static class ConnectionCheckTool
{
    private static Dictionary<ConnectionCheckType, List<string>> map;
    static ConnectionCheckTool()
    {
        map = new Dictionary<ConnectionCheckType, List<string>>();
        #region
        map.Add(ConnectionCheckType.�����ײ������ն�ֱ�ӽӶ�·�岻�Ӷ��˿�����ʱ,
            new List<string>() { "΢���ź�Դ", "����תͬ��", "������", "�ɱ�˥����", "�����ײ�����" });
        #endregion
    }

}
public enum ConnectionCheckType
{
    �����ײ������ն�ֱ�ӽӶ�·�岻�Ӷ��˿�����ʱ,
}