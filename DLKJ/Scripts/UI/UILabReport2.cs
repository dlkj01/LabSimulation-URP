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

        Dictionary<string, object> map1 = WordHelper.GetFields(userData);
        Dictionary<string, object> map2 = WordHelper.GetFields(labReport2Data);
        foreach (var item in map1)
            map[item.Key] = item.Value;
        //foreach (var item2 in map2)
        //    map[item2.Key] = item2.Value;

        foreach (var item2 in map2)
        {
            AnswerCheck answerCheck = new AnswerCheck();
            answerCheck.answer = item2.Value.ToString();
            System.Reflection.FieldInfo[] fields = MathTool.report1CorrectAnswer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
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
                    answerCheck.isRight = DataFormatParsing(result, fields[i].GetValue(MathTool.report1CorrectAnswer));
                }
            }
            map[item2.Key] = answerCheck;
        }
        WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
    /// <summary>
    /// 缓存数据
    /// </summary>
    public void CacheFirstGroupData()
    {
        WordHelper.cacheUserData.userName = nameInputField.text;
        WordHelper.cacheUserData.id = idInputField.text;
        WordHelper.cacheUserData.className = classInputField.text;
        WordHelper.cacheUserData.teacherName = teacherInputField.text;
        WordHelper.cacheUserData.time = timeInputField.text;


        WordHelper.cacheData.inputSourceFrequencyFirst = StringToDouble(inputSourceFrequencyFirst.text);
        WordHelper.cacheData.inputSourceVoltageFirst = StringToDouble(inputSourceVoltageFirst.text);
        WordHelper.cacheData.inputAttenuatorSetupFirst = StringToDouble(inputAttenuatorSetupFirst.text);
        WordHelper.cacheData.SWRFirst = StringToDouble(SWRFirst.text);
        WordHelper.cacheData.WaveguideWavelengthFirst = StringToDouble(WaveguideWavelengthFirst.text);
        WordHelper.cacheData.EquivalentSectionPositionSecond = StringToDouble(EquivalentSectionPositionSecond.text);
        WordHelper.cacheData.WaveNodePositionFirst = StringToDouble(WaveNodePositionFirst.text);
        WordHelper.cacheData.NormalizedLoadImpedanceFirst = StringToDouble(NormalizedLoadImpedanceFirst.text);
        WordHelper.cacheData.LoadImpedanceFirst = StringToDouble(LoadImpedanceFirst.text);
        WordHelper.cacheData.ScrewPositionFirst = StringToDouble(ScrewPositionFirst.text);
        WordHelper.cacheData.ScrewDepthFirst = StringToDouble(ScrewDepthFirst.text);
        WordHelper.cacheData.MinimumVoltageAfterMatchingFirst = StringToDouble(MinimumVoltageAfterMatchingFirst.text);
        WordHelper.cacheData.MaximumVoltageAfterMatchingFirst = StringToDouble(MaximumVoltageAfterMatchingFirst.text);
        WordHelper.cacheData.SWRAfterMatchingFirst = StringToDouble(SWRAfterMatchingFirst.text);
    }
    public void WriteInputText(LabReport2Data data, UserDate userData)
    {
        DisposeResult(userData.userName, nameInputField, true);
        DisposeResult(userData.className, classInputField, true);
        DisposeResult(userData.time, timeInputField, true);
        DisposeResult(userData.id, idInputField, true);
        DisposeResult(userData.teacherName, teacherInputField, true);

        DisposeResult(data.inputSourceFrequencyFirst.ToString(), inputSourceFrequencyFirst);
        DisposeResult(data.inputSourceVoltageFirst.ToString(), inputSourceVoltageFirst);
        DisposeResult(data.inputAttenuatorSetupFirst.ToString(), inputAttenuatorSetupFirst);
        DisposeResult(data.SWRFirst.ToString(), SWRFirst);
        DisposeResult(data.WaveguideWavelengthFirst.ToString(), WaveguideWavelengthFirst);
        DisposeResult(data.EquivalentSectionPositionFirst.ToString(), EquivalentSectionPositionFirst);
        DisposeResult(data.WaveNodePositionFirst.ToString(), WaveNodePositionFirst);
        DisposeResult(data.NormalizedLoadImpedanceFirst.ToString(), NormalizedLoadImpedanceFirst);
        DisposeResult(data.LoadImpedanceFirst.ToString(), LoadImpedanceFirst);
        DisposeResult(data.ScrewPositionFirst.ToString(), ScrewPositionFirst);
        DisposeResult(data.ScrewDepthFirst.ToString(), ScrewDepthFirst);
        DisposeResult(data.MinimumVoltageAfterMatchingFirst.ToString(), MinimumVoltageAfterMatchingFirst);
        DisposeResult(data.MaximumVoltageAfterMatchingFirst.ToString(), MaximumVoltageAfterMatchingFirst);
        DisposeResult(data.SWRAfterMatchingFirst.ToString(), SWRAfterMatchingFirst);
    }

    private void DisposeResult(string result, InputField field, bool canChange = false)
    {
        string resultText = "";
        if (result == "-9999")
            resultText = "";
        else
            resultText = result;
        field.text = resultText;
        if (canChange == false)
        {
            field.interactable = false;
        }
    }
}
