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
    [Header("无情分割线---------------------------------------------------------")]
    [Space(10)]
    [Tooltip("信号源频率")] public InputField inputSourceFrequency;//信号源频率
    [Tooltip("信号源电压")] public InputField inputSourceVoltage;//信号源电压
    [Tooltip("一端口电压")] public InputField OnePortVoltage;//一端口电压
    [Tooltip("三端口电压")] public InputField ThreePortVoltage;//三端口电压
    [Tooltip("三端口电压")] public InputField CouplingFactor;//耦合度C

    public override void SaveData()
    {
        base.SaveData();
        labReportData.inputSourceFrequency = StringToDouble(inputSourceFrequency.text);
        labReportData.inputSourceVoltage = StringToDouble(inputSourceVoltage.text);
        labReportData.OnePortVoltage = StringToDouble(OnePortVoltage.text);
        labReportData.ThreePortVoltage = StringToDouble(ThreePortVoltage.text);
        labReportData.CouplingFactor = StringToDouble(CouplingFactor.text);
        AddResult(labReportData, MathTool.report3CorrectAnswer);
    }
}
