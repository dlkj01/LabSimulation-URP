using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILabReport3 : UILabReportBase
{
    public LabReport3Data labReportData = new LabReport3Data();
    [Tooltip("һ�˿ڵ�ѹ")] public InputField OnePortVoltage;//һ�˿ڵ�ѹ
    [Tooltip("���˿ڵ�ѹ")] public InputField ThreePortVoltage;//���˿ڵ�ѹ
    [Tooltip("���˿ڵ�ѹ")] public InputField CouplingFactor;//��϶�C

    public override void SaveData()
    {
        base.SaveData();
        labReportData.OnePortVoltage = StringToDouble(OnePortVoltage.text);
        labReportData.ThreePortVoltage = StringToDouble(ThreePortVoltage.text);
        labReportData.CouplingFactor = StringToDouble(CouplingFactor.text);
        Dictionary<string, object> map1 = WordHelper.GetFields(userData);
        Dictionary<string, object> map2 = WordHelper.GetFields(labReportData);
        foreach (var item in map1)
            map[item.Key] = item.Value;
        foreach (var item2 in map2)
            map[item2.Key] = item2.Value;

        WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
}
