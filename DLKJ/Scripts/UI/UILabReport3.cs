using DLKJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class UILabReport3 : UILabReportBase
{
    public LabReport3Data labReportData = new LabReport3Data();
    [Tooltip("一端口电压")] public InputField OnePortVoltage;//一端口电压
    [Tooltip("三端口电压")] public InputField ThreePortVoltage;//三端口电压
    [Tooltip("三端口电压")] public InputField CouplingFactor;//耦合度C

    public override void SaveData()
    {
        base.SaveData();
        labReportData.OnePortVoltage = StringToDouble(OnePortVoltage.text);
        labReportData.ThreePortVoltage = StringToDouble(ThreePortVoltage.text);
        labReportData.CouplingFactor = StringToDouble(CouplingFactor.text);
        AddResult(labReportData, MathTool.report3CorrectAnswer);
    }
}
