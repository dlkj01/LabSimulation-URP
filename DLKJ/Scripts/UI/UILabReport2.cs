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
    [Tooltip("�ź�ԴƵ��")]
    [Header("��һ��------------------------------------------------")]
    public InputField inputSourceFrequencyFirst;//�ź�ԴƵ��

    [Tooltip("�ź�Դ��ѹ")] public InputField inputSourceVoltageFirst;//�ź�Դ��ѹ

    [Tooltip("˥��������")] public InputField inputAttenuatorSetupFirst;//˥��������

    [Tooltip("פ����")] public InputField SWRFirst;//פ����

    [Tooltip("��������")] public InputField WaveguideWavelengthFirst;//��������

    [SerializeField] InputField EquivalentSectionPositionFirst;//��Ч����λ��

    [Tooltip("��С��ѹ")] public InputField MinimumVoltageInput;

    [Tooltip("��ĵ�ѹ")] public InputField MaximumVoltageInput;

    [Tooltip("��һ���ڵ�λ��")] public InputField WaveNodePositionFirst;//��һ���ڵ�λ��

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceFirst;//��һ�������迹

    [Tooltip("�����迹")] public InputField LoadImpedanceFirst;//�����迹

    [Tooltip("�ݶ�λ��")] public InputField ScrewPositionFirst;//�ݶ�λ��

    [Tooltip("�ݶ����")] public InputField ScrewDepthFirst;//�ݶ����

    [Tooltip("ƥ�����С��ѹ")] public InputField MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ

    [Tooltip("ƥ�������ѹ")] public InputField MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ

    [Tooltip("ƥ���פ����")] public InputField SWRAfterMatchingFirst;//ƥ���פ����

    public override void SaveData()
    {
        base.SaveData();
        labReport2Data.inputSourceFrequencyFirst = StringToDouble(inputSourceFrequencyFirst.text);
        labReport2Data.inputSourceVoltageFirst = StringToDouble(inputSourceVoltageFirst.text);
        labReport2Data.inputAttenuatorSetupFirst = StringToDouble(inputAttenuatorSetupFirst.text);
        labReport2Data.SWRFirst = StringToDouble(SWRFirst.text);
        labReport2Data.WaveguideWavelengthFirst = StringToDouble(WaveguideWavelengthFirst.text) * 0.001f;
        labReport2Data.MinimumVoltage = StringToDouble(MinimumVoltageInput.text);
        labReport2Data.MaximumVoltage = StringToDouble(MaximumVoltageInput.text);
        labReport2Data.EquivalentSectionPositionFirst = StringToDouble(EquivalentSectionPositionFirst.text) * 0.001f;
        labReport2Data.WaveNodePositionFirst = StringToDouble(WaveNodePositionFirst.text);
        labReport2Data.NormalizedLoadImpedanceFirst = StringToDouble(NormalizedLoadImpedanceFirst.text);
        labReport2Data.LoadImpedanceFirst = StringToDouble(LoadImpedanceFirst.text);
        labReport2Data.ScrewPositionFirst = StringToDouble(ScrewPositionFirst.text) * 0.001f;
        labReport2Data.ScrewDepthFirst = StringToDouble(ScrewDepthFirst.text) * 0.001f;
        labReport2Data.MinimumVoltageAfterMatchingFirst = StringToDouble(MinimumVoltageAfterMatchingFirst.text);
        labReport2Data.MaximumVoltageAfterMatchingFirst = StringToDouble(MaximumVoltageAfterMatchingFirst.text);
        labReport2Data.SWRAfterMatchingFirst = StringToDouble(SWRAfterMatchingFirst.text);
        AddResult(labReport2Data, MathTool.report2CorrectAnswer);
    }
}
