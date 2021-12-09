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
        [SerializeField] InputField ReflectionCoefficient��1SReal;//����ϵ����·
        [SerializeField] InputField ReflectionCoefficient��10Real;//����ϵ���ն�
        [SerializeField] InputField ReflectionCoefficient��1LReal;//����ϵ��ƥ��
        [SerializeField] InputField ReflectionCoefficient��1SImaginary;//����ϵ����·
        [SerializeField] InputField ReflectionCoefficient��10Imaginary;//����ϵ���ն�
        [SerializeField] InputField ReflectionCoefficient��1LImaginary;//����ϵ��ƥ��
        [SerializeField] InputField inputS11Real;
        [SerializeField] InputField inputS11Imaginary;
        [SerializeField] InputField inputS12S21Real;
        [SerializeField] InputField inputS12S21Imaginary;
        [SerializeField] InputField inputS22Real;
        [SerializeField] InputField inputS22Imaginary;

        public override void SaveData()
        {
            base.SaveData();
            labReport1Data.SourceFrequency = StringToDouble(SourceFrequency.text);//�ź�ԴƵ��
            labReport1Data.SourceVoltage = StringToDouble(SourceVoltage.text);//�ź�Դ��ѹ����
            labReport1Data.Attenuator = StringToDouble(Attenuator.text);//˥��������
            labReport1Data.EquivalentSectionPosition = StringToDouble(EquivalentSectionPosition.text);//��Ч����λ��
            labReport1Data.InputWavelength = StringToDouble(InputWavelength.text);//����˲���
            labReport1Data.VariableShortCircuitFirstPos = StringToDouble(VariableShortCircuitFirstPos.text);//�ɱ��·����һ���ڵ�λ��
            labReport1Data.VariableShortCircuitSecondPos = StringToDouble(VariableShortCircuitSecondPos.text);//�ɱ��·���ڶ����ڵ�λ��
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
            labReport1Data.ReflectionCoefficient��1SReal = StringToDouble(ReflectionCoefficient��1SReal.text);//����ϵ����·
            labReport1Data.ReflectionCoefficient��10Real = StringToDouble(ReflectionCoefficient��10Real.text);//����ϵ���ն�
            labReport1Data.ReflectionCoefficient��1LReal = StringToDouble(ReflectionCoefficient��1LReal.text);//����ϵ��ƥ��
            labReport1Data.ReflectionCoefficient��1SImaginary = StringToDouble(ReflectionCoefficient��1SImaginary.text);//����ϵ����·
            labReport1Data.ReflectionCoefficient��10Imaginary = StringToDouble(ReflectionCoefficient��10Imaginary.text);//����ϵ���ն�
            labReport1Data.ReflectionCoefficient��1LImaginary = StringToDouble(ReflectionCoefficient��1LImaginary.text);//����ϵ��ƥ��
            labReport1Data.inputS11Real = StringToDouble(inputS11Real.text);
            labReport1Data.inputS11Imaginary = StringToDouble(inputS11Imaginary.text);
            labReport1Data.inputS12S21Real = StringToDouble(inputS12S21Real.text);
            labReport1Data.inputS12S21Imaginary = StringToDouble(inputS12S21Imaginary.text);
            labReport1Data.inputS22Real = StringToDouble(inputS22Real.text);
            labReport1Data.inputS22Imaginary = StringToDouble(inputS22Imaginary.text);
            AddResult(labReport1Data, MathTool.report1CorrectAnswer);
        }

        public override void SetInputTextReadOnly()
        {
            base.SetInputTextReadOnly();
            SourceFrequency.text = MathTool.F.ToString("#0.00");
            SourceVoltage.text = MathTool.A.ToString("#0.00");
            Attenuator.text = (1 - MathTool.��).ToString("#0.00");
            SourceFrequency.readOnly = true;
            SourceVoltage.readOnly = true;
            Attenuator.readOnly = true;
        }
    }
}