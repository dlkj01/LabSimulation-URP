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
        {
            AnswerCheck answerCheck = new AnswerCheck();
            answerCheck.answer = item2.Value.ToString();
            System.Reflection.FieldInfo[] fields = MathTool.report3CorrectAnswer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == item2.Key)
                {
                    double result;
                    if (String.IsNullOrEmpty(item2.Value.ToString()))
                    {
                        result = -9999;
                    }
                    else
                    {
                        result = (double)item2.Value;
                    }
                    answerCheck.isRight = DataFormatParsing(result, fields[i].GetValue(MathTool.report3CorrectAnswer));
                }
            }
            map[item2.Key] = answerCheck;
        }
        WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
}
