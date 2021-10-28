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

    [Tooltip("��һ���ڵ�λ��")] public InputField WaveNodePositionFirst;//��һ���ڵ�λ��

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceFirst;//��һ�������迹

    [Tooltip("�����迹")] public InputField LoadImpedanceFirst;//�����迹

    [Tooltip("�ݶ�λ��")] public InputField ScrewPositionFirst;//�ݶ�λ��

    [Tooltip("�ݶ����")] public InputField ScrewDepthFirst;//�ݶ����

    [Tooltip("ƥ�����С��ѹ")] public InputField MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ

    [Tooltip("ƥ�������ѹ")] public InputField MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ

    [Tooltip("ƥ���פ����")] public InputField SWRAfterMatchingFirst;//ƥ���פ����

    [Tooltip("�ź�ԴƵ��")]
    [Header("�ڶ���------------------------------------------------")]
    public InputField inputSourceFrequencySecond;//�ź�ԴƵ��

    [Tooltip("�ź�Դ��ѹ")] public InputField inputSourceVoltageSecond;//�ź�Դ��ѹ

    [Tooltip("˥��������")] public InputField inputAttenuatorSetupSecond;//˥��������

    [Tooltip("פ����")] public InputField SWRSecond;//פ����

    [Tooltip("��������")] public InputField WaveguideWavelengthSecond;//��������

    [SerializeField] InputField EquivalentSectionPositionSecond;//��Ч����λ��

    [Tooltip("��һ���ڵ�λ��")] public InputField WaveNodePositionSecond;//��һ���ڵ�λ��

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceSecond;//��һ�������迹

    [Tooltip("�����迹")] public InputField LoadImpedanceSecond;//�����迹

    [Tooltip("�ݶ�λ��")] public InputField ScrewPositionSecond;//�ݶ�λ��

    [Tooltip("�ݶ����")] public InputField ScrewDepthSecond;//�ݶ����

    [Tooltip("ƥ�����С��ѹ")] public InputField MinimumVoltageAfterMatchingSecond;//ƥ�����С��ѹ

    [Tooltip("ƥ�������ѹ")] public InputField MaximumVoltageAfterMatchingSecond;//ƥ�������ѹ

    [Tooltip("ƥ���פ����")] public InputField SWRAfterMatchingSecond;//ƥ���פ����


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
    /// ���õ�һ�����ݲ��ɽ���
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
