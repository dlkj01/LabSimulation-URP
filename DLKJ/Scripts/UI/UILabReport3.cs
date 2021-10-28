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
        // Dictionary<string, object> map2 = WordHelper.GetFields(labReportData);
        AddResult(labReportData);

        //foreach (var item2 in map2)
        //{
        //    AnswerCheck answerCheck = new AnswerCheck();
        //    answerCheck.answer = item2.Value.ToString();
        //    System.Reflection.FieldInfo[] fields = MathTool.report3CorrectAnswer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        //    for (int i = 0; i < fields.Length; i++)
        //    {
        //        if (fields[i].Name == item2.Key)
        //        {
        //            double result;
        //            if (String.IsNullOrEmpty(item2.Value.ToString()))
        //            {
        //                result = -9999;
        //            }
        //            else
        //            {
        //                result = (double)item2.Value;
        //            }
        //            answerCheck.isRight = DataFormatParsing(result, fields[i].GetValue(MathTool.report3CorrectAnswer));
        //        }
        //    }
        //    map[item2.Key] = answerCheck;
        //}
        //WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
}
