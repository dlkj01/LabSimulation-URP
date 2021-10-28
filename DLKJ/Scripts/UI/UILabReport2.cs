using DLKJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
public class UILabReport2 : UILabReportBase
{
    public LabReport2Data labReport2Data = new LabReport2Data();
    [Tooltip("信号源频率")]
    [Header("第一组------------------------------------------------")]
    public InputField inputSourceFrequencyFirst;//信号源频率

    [Tooltip("信号源电压")] public InputField inputSourceVoltageFirst;//信号源电压

    [Tooltip("衰减器设置")] public InputField inputAttenuatorSetupFirst;//衰减器设置

    [Tooltip("驻波比")] public InputField SWRFirst;//驻波比

    [Tooltip("波导波长")] public InputField WaveguideWavelengthFirst;//波导波长

    [SerializeField] InputField EquivalentSectionPositionFirst;//等效截面位置

    [Tooltip("第一波节点位置")] public InputField WaveNodePositionFirst;//第一波节点位置

    [Tooltip("归一化负载阻抗")] public InputField NormalizedLoadImpedanceFirst;//归一化负载阻抗

    [Tooltip("负载阻抗")] public InputField LoadImpedanceFirst;//负载阻抗

    [Tooltip("螺钉位置")] public InputField ScrewPositionFirst;//螺钉位置

    [Tooltip("螺钉深度")] public InputField ScrewDepthFirst;//螺钉深度

    [Tooltip("匹配后最小电压")] public InputField MinimumVoltageAfterMatchingFirst;//匹配后最小电压

    [Tooltip("匹配后最大电压")] public InputField MaximumVoltageAfterMatchingFirst;//匹配后最大电压

    [Tooltip("匹配后驻波比")] public InputField SWRAfterMatchingFirst;//匹配后驻波比

    [Tooltip("信号源频率")]
    [Header("第二组------------------------------------------------")]
    public InputField inputSourceFrequencySecond;//信号源频率

    [Tooltip("信号源电压")] public InputField inputSourceVoltageSecond;//信号源电压

    [Tooltip("衰减器设置")] public InputField inputAttenuatorSetupSecond;//衰减器设置

    [Tooltip("驻波比")] public InputField SWRSecond;//驻波比

    [Tooltip("波导波长")] public InputField WaveguideWavelengthSecond;//波导波长

    [SerializeField] InputField EquivalentSectionPositionSecond;//等效截面位置

    [Tooltip("第一波节点位置")] public InputField WaveNodePositionSecond;//第一波节点位置

    [Tooltip("归一化负载阻抗")] public InputField NormalizedLoadImpedanceSecond;//归一化负载阻抗

    [Tooltip("负载阻抗")] public InputField LoadImpedanceSecond;//负载阻抗

    [Tooltip("螺钉位置")] public InputField ScrewPositionSecond;//螺钉位置

    [Tooltip("螺钉深度")] public InputField ScrewDepthSecond;//螺钉深度

    [Tooltip("匹配后最小电压")] public InputField MinimumVoltageAfterMatchingSecond;//匹配后最小电压

    [Tooltip("匹配后最大电压")] public InputField MaximumVoltageAfterMatchingSecond;//匹配后最大电压

    [Tooltip("匹配后驻波比")] public InputField SWRAfterMatchingSecond;//匹配后驻波比


    public override void SaveData()
    {
        base.SaveData();
        labReport2Data.inputSourceFrequencyFirst = StringToDouble(inputSourceFrequencyFirst.text);
        labReport2Data.inputSourceVoltageFirst = StringToDouble(inputSourceVoltageFirst.text);
        labReport2Data.inputAttenuatorSetupFirst = StringToDouble(inputAttenuatorSetupFirst.text);
        labReport2Data.SWRFirst = StringToDouble(SWRFirst.text);
        labReport2Data.WaveguideWavelengthFirst = StringToDouble(WaveguideWavelengthFirst.text);
        labReport2Data.EquivalentSectionPositionFirst = StringToDouble(EquivalentSectionPositionFirst.text);
        labReport2Data.WaveNodePositionFirst = StringToDouble(WaveNodePositionFirst.text);
        labReport2Data.NormalizedLoadImpedanceFirst = StringToDouble(NormalizedLoadImpedanceFirst.text);
        labReport2Data.LoadImpedanceFirst = StringToDouble(LoadImpedanceFirst.text);
        labReport2Data.ScrewPositionFirst = StringToDouble(ScrewPositionFirst.text);
        labReport2Data.ScrewDepthFirst = StringToDouble(ScrewDepthFirst.text);
        labReport2Data.MinimumVoltageAfterMatchingFirst = StringToDouble(MinimumVoltageAfterMatchingFirst.text);
        labReport2Data.MaximumVoltageAfterMatchingFirst = StringToDouble(MaximumVoltageAfterMatchingFirst.text);
        labReport2Data.SWRAfterMatchingFirst = StringToDouble(SWRAfterMatchingFirst.text);
        labReport2Data.inputSourceFrequencySecond = StringToDouble(inputSourceFrequencySecond.text);
        labReport2Data.inputSourceVoltageSecond = StringToDouble(inputSourceVoltageSecond.text);
        labReport2Data.inputAttenuatorSetupSecond = StringToDouble(inputAttenuatorSetupSecond.text);
        labReport2Data.SWRSecond = StringToDouble(SWRSecond.text);
        labReport2Data.WaveguideWavelengthSecond = StringToDouble(WaveguideWavelengthSecond.text);
        labReport2Data.EquivalentSectionPositionSecond = StringToDouble(EquivalentSectionPositionSecond.text);
        labReport2Data.WaveNodePositionSecond = StringToDouble(WaveNodePositionSecond.text);
        labReport2Data.NormalizedLoadImpedanceSecond = StringToDouble(NormalizedLoadImpedanceSecond.text);
        labReport2Data.LoadImpedanceSecond = StringToDouble(LoadImpedanceSecond.text);
        labReport2Data.ScrewPositionSecond = StringToDouble(ScrewPositionSecond.text);
        labReport2Data.ScrewDepthSecond = StringToDouble(ScrewDepthSecond.text);
        labReport2Data.MinimumVoltageAfterMatchingSecond = StringToDouble(MinimumVoltageAfterMatchingSecond.text);
        labReport2Data.MaximumVoltageAfterMatchingSecond = StringToDouble(MaximumVoltageAfterMatchingSecond.text);
        labReport2Data.SWRAfterMatchingSecond = StringToDouble(SWRAfterMatchingSecond.text);


        // Dictionary<string, object> map2 = WordHelper.GetFields(labReport2Data);

        //foreach (var item2 in map2)
        //    map[item2.Key] = item2.Value;
        AddResult(labReport2Data);
        //foreach (var item2 in map2)
        //{
        //    AnswerCheck answerCheck = new AnswerCheck();
        //    answerCheck.answer = item2.Value.ToString();
        //    System.Reflection.FieldInfo[] fields = MathTool.report1CorrectAnswer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
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
        //            answerCheck.isRight = DataFormatParsing(result, fields[i].GetValue(MathTool.report1CorrectAnswer));
        //        }
        //    }
        //    map[item2.Key] = answerCheck;
        //}
        //WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
    /// <summary>
    /// 设置第一组数据不可交互
    /// </summary>
    public void WriteInputText()
    {
        DisposeResult(inputSourceFrequencyFirst);
        DisposeResult(inputSourceVoltageFirst);
        DisposeResult(inputAttenuatorSetupFirst);
        DisposeResult(SWRFirst);
        DisposeResult(WaveguideWavelengthFirst);
        DisposeResult(EquivalentSectionPositionFirst);
        DisposeResult(WaveNodePositionFirst);
        DisposeResult(NormalizedLoadImpedanceFirst);
        DisposeResult(LoadImpedanceFirst);
        DisposeResult(ScrewPositionFirst);
        DisposeResult(ScrewDepthFirst);
        DisposeResult(MinimumVoltageAfterMatchingFirst);
        DisposeResult(MaximumVoltageAfterMatchingFirst);
        DisposeResult(SWRAfterMatchingFirst);
    }

    private void DisposeResult(InputField field, bool canChange = false)
    {
        if (canChange == false)
        {
            field.interactable = false;
        }
    }
}
