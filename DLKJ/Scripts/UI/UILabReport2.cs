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

    [Tooltip("最小电压")] public InputField MinimumVoltageInput;

    [Tooltip("最爱的电压")] public InputField MaximumVoltageInput;

    [Tooltip("第一波节点位置")] public InputField WaveNodePositionFirst;//第一波节点位置

    [Tooltip("归一化负载阻抗")] public InputField NormalizedLoadImpedanceFirst;//归一化负载阻抗

    [Tooltip("负载阻抗")] public InputField LoadImpedanceFirstReal;//负载阻抗电阻

    [Tooltip("负载阻抗")] public InputField LoadImpedanceFirstImaginary;//负载阻抗电抗

    [Tooltip("螺钉位置")] public InputField ScrewPositionFirst;//螺钉位置

    [Tooltip("螺钉深度")] public InputField ScrewDepthFirst;//螺钉深度

    [Tooltip("匹配后最小电压")] public InputField MinimumVoltageAfterMatchingFirst;//匹配后最小电压

    [Tooltip("匹配后最大电压")] public InputField MaximumVoltageAfterMatchingFirst;//匹配后最大电压

    [Tooltip("匹配后驻波比")] public InputField SWRAfterMatchingFirst;//匹配后驻波比

    public override void SaveData()
    {
        base.SaveData();
        labReport2Data.inputSourceFrequencyFirst = StringToDouble(inputSourceFrequencyFirst.text);
        labReport2Data.inputSourceVoltageFirst = StringToDouble(inputSourceVoltageFirst.text);
        labReport2Data.inputAttenuatorSetupFirst = StringToDouble(inputAttenuatorSetupFirst.text);
        labReport2Data.SWRFirst = StringToDouble(SWRFirst.text);
        labReport2Data.WaveguideWavelengthFirst = StringToDouble(WaveguideWavelengthFirst.text);
        labReport2Data.MinimumVoltage = StringToDouble(MinimumVoltageInput.text);
        labReport2Data.MaximumVoltage = StringToDouble(MaximumVoltageInput.text);
        labReport2Data.EquivalentSectionPositionFirst = StringToDouble(EquivalentSectionPositionFirst.text);
        labReport2Data.WaveNodePositionFirst = StringToDouble(WaveNodePositionFirst.text);
        labReport2Data.NormalizedLoadImpedanceFirst = StringToDouble(NormalizedLoadImpedanceFirst.text);
        labReport2Data.LoadImpedanceFirstReal = StringToDouble(LoadImpedanceFirstReal.text);
        labReport2Data.LoadImpedanceFirstImaginary = StringToDouble(LoadImpedanceFirstImaginary.text);
        labReport2Data.ScrewPositionFirst = StringToDouble(ScrewPositionFirst.text);
        labReport2Data.ScrewDepthFirst = StringToDouble(ScrewDepthFirst.text);
        labReport2Data.MinimumVoltageAfterMatchingFirst = StringToDouble(MinimumVoltageAfterMatchingFirst.text);
        labReport2Data.MaximumVoltageAfterMatchingFirst = StringToDouble(MaximumVoltageAfterMatchingFirst.text);
        labReport2Data.SWRAfterMatchingFirst = StringToDouble(SWRAfterMatchingFirst.text);
        AddResult(labReport2Data, MathTool.report2CorrectAnswer);
    }
}
