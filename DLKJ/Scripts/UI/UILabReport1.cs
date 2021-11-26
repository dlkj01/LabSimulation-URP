using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class UILabReport1 : UILabReportBase
    {

        public LabReport1Data labReport1Data = new LabReport1Data();
        public void OnSureCallBack()
        {
            if (nameInputField.text.Length > 0)
            {

            }
            else
            {
                EventManager.OnTips(TipsType.Toast, "����������");
            }
        }

        [Space()]
        [Header("����ָ���---------------------------------------------------")]
        [SerializeField] InputField SourceFrequency;//�ź�ԴƵ��
        [SerializeField] InputField SourceVoltage;//�ź�Դ��ѹ����
        [SerializeField] InputField Attenuator;//˥��������
        [SerializeField] InputField EquivalentSectionPosition;//��Ч����λ��
        [SerializeField] InputField InputWavelength;//����˲���
        [SerializeField] InputField VariableShortCircuitFirstPos;//�ɱ��·����һ���ڵ�λ��
        [SerializeField] InputField VariableShortCircuitSecondPos;//�ɱ��·���ڶ����ڵ�λ��
        [SerializeField] InputField VariableWavelengthInShortCircuit;//�ɱ��·���в��� 
        [SerializeField] InputField OpenLoadPosition;//��·����λ��
        [SerializeField] InputField WaveNodePosShortCircuit;//���ڵ�λ�ö�·
        [SerializeField] InputField WaveNodePosShortTerminal;//���ڵ�λ���ն�
        [SerializeField] InputField WaveNodePosShortMatching;//���ڵ�λ��ƥ��
        [SerializeField] InputField PhaseAngleCircuit;//��Ƕ�·
        [SerializeField] InputField PhaseAngleTerminal;//����ն�
        [SerializeField] InputField PhaseAngleMatching;//���ƥ��
        [SerializeField] InputField StandingWaveRatioCircuit;//פ���ȶ�·
        [SerializeField] InputField StandingWaveRatioTerminal;//פ�����ն�
        [SerializeField] InputField StandingWaveRatioMatching;//פ����ƥ��
        [SerializeField] InputField input��1S;
        [SerializeField] InputField input��10;
        [SerializeField] InputField input��1L;
        [SerializeField] InputField ReflectionCoefficient��1S;//����ϵ����·
        [SerializeField] InputField ReflectionCoefficient��10;//����ϵ���ն�
        [SerializeField] InputField ReflectionCoefficient��1L;//����ϵ��ƥ��
        [SerializeField] InputField inputS11;
        [SerializeField] InputField inputS12S21;
        [SerializeField] InputField inputS22;

        public override void SaveData()
        {
            base.SaveData();
            labReport1Data.SourceFrequency = StringToDouble(SourceFrequency.text);//�ź�ԴƵ��
            labReport1Data.SourceVoltage = StringToDouble(SourceVoltage.text);//�ź�Դ��ѹ����
            labReport1Data.Attenuator = StringToDouble(Attenuator.text);//˥��������
            labReport1Data.EquivalentSectionPosition = StringToDouble(EquivalentSectionPosition.text);//��Ч����λ��
            labReport1Data.InputWavelength = StringToDouble(InputWavelength.text);//����˲���
            labReport1Data.VariableShortCircuitFirstPos = StringToDouble(VariableShortCircuitFirstPos.text);//�ɱ��·����һ���ڵ�λ��
            labReport1Data.VariableShortCircuitSecondPos = StringToDouble(VariableShortCircuitSecondPos.text) ;//�ɱ��·���ڶ����ڵ�λ��
            labReport1Data.VariableWavelengthInShortCircuit = StringToDouble(VariableWavelengthInShortCircuit.text);//�ɱ��·���в��� 
            labReport1Data.OpenLoadPosition = StringToDouble(OpenLoadPosition.text);//��·����λ��
            labReport1Data.WaveNodePosShortCircuit = StringToDouble(WaveNodePosShortCircuit.text);//���ڵ�λ�ö�·
            labReport1Data.WaveNodePosShortTerminal = StringToDouble(WaveNodePosShortTerminal.text);//���ڵ�λ���ն�
            labReport1Data.WaveNodePosShortMatching = StringToDouble(WaveNodePosShortMatching.text);//���ڵ�λ��ƥ��
            labReport1Data.PhaseAngleCircuit = StringToDouble(PhaseAngleCircuit.text);//��Ƕ�·
            labReport1Data.PhaseAngleTerminal = StringToDouble(PhaseAngleTerminal.text);//����ն�
            labReport1Data.PhaseAngleMatching = StringToDouble(PhaseAngleMatching.text);//���ƥ��
            labReport1Data.StandingWaveRatioCircuit = StringToDouble(StandingWaveRatioCircuit.text);//פ���ȶ�·
            labReport1Data.StandingWaveRatioTerminal = StringToDouble(StandingWaveRatioTerminal.text);//פ�����ն�
            labReport1Data.StandingWaveRatioMatching = StringToDouble(StandingWaveRatioMatching.text);//פ����ƥ��
            labReport1Data.input��1S = StringToDouble(input��1S.text);
            labReport1Data.input��10 = StringToDouble(input��10.text);
            labReport1Data.input��1L = StringToDouble(input��1L.text);
            labReport1Data.ReflectionCoefficient��1S = StringToDouble(ReflectionCoefficient��1S.text);//����ϵ����·
            labReport1Data.ReflectionCoefficient��10 = StringToDouble(ReflectionCoefficient��10.text);//����ϵ���ն�
            labReport1Data.ReflectionCoefficient��1L = StringToDouble(ReflectionCoefficient��1L.text);//����ϵ��ƥ��
            labReport1Data.inputS11 = StringToDouble(inputS11.text);
            labReport1Data.inputS12S21 = StringToDouble(inputS12S21.text);
            labReport1Data.inputS22 = StringToDouble(inputS22.text);
            AddResult(labReport1Data, MathTool.report1CorrectAnswer);
        }
    }
}