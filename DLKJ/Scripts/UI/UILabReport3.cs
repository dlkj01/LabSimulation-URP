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
    [Header("����ָ���---------------------------------------------------------")]
    [Space(10)]
    [Tooltip("�ź�ԴƵ��")] public InputField inputSourceFrequency;//�ź�ԴƵ��
    [Tooltip("�ź�Դ��ѹ")] public InputField inputSourceVoltage;//�ź�Դ��ѹ
    [Tooltip("һ�˿ڵ�ѹ")] public InputField OnePortVoltage;//һ�˿ڵ�ѹ
    [Tooltip("���˿ڵ�ѹ")] public InputField ThreePortVoltage;//���˿ڵ�ѹ
    [Tooltip("���˿ڵ�ѹ")] public InputField CouplingFactor;//��϶�C

    public override void SaveData()
    {
        base.SaveData();
        labReportData.OnePortVoltage = StringToDouble(OnePortVoltage.text);
        labReportData.ThreePortVoltage = StringToDouble(ThreePortVoltage.text);
        labReportData.CouplingFactor = StringToDouble(CouplingFactor.text);
        AddResult(labReportData, MathTool.report3CorrectAnswer);
        labReportData.inputSourceFrequency = StringToDouble(inputSourceFrequency.text);
        labReportData.inputSourceVoltage = StringToDouble(inputSourceVoltage.text);
    }
    public override void SetInputTextReadOnly()
    {
        base.SetInputTextReadOnly();
        inputSourceFrequency.text = MathTool.F.ToString("#0.00");
        inputSourceVoltage.text = MathTool.A.ToString("#0.00");
        inputSourceFrequency.readOnly = true;
        inputSourceVoltage.readOnly = true;
    }
}
