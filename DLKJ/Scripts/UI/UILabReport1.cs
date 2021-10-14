using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class UILabReport1 : UILabReportBase
    {
        [SerializeField] InputField nameInputField;
        [SerializeField] InputField classInputField;
        [SerializeField] InputField timeInputField;
        [SerializeField] InputField idInputField;
        [SerializeField] InputField teacherInputField;

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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SetVisibale(false);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SetVisibale(true);
            }
        }
    }
}