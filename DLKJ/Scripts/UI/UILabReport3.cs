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

    protected override void SaveData()
    {
        base.SaveData();
        labReportData.OnePortVoltage = StringToDouble(OnePortVoltage.text);
        labReportData.ThreePortVoltage = StringToDouble(ThreePortVoltage.text);
        labReportData.CouplingFactor = StringToDouble(CouplingFactor.text);
    }
}
