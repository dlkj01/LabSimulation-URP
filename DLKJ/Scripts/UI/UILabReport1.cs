
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
                EventManager.OnTips(TipsType.Toast, "请输入姓名");
            }
        }

        [Space()]
        [Header("无情分割线---------------------------------------------------")]
        [SerializeField] InputField SourceFrequency;//信号源频率
        [SerializeField] InputField SourceVoltage;//信号源电压设置
        [SerializeField] InputField Attenuator;//衰减器设置
        [SerializeField] InputField EquivalentSectionPosition;//等效截面位置
        [SerializeField] InputField InputWavelength;//输入端波长
        [SerializeField] InputField VariableShortCircuitFirstPos;//可变短路器第一波节点位置
        [SerializeField] InputField VariableShortCircuitSecondPos;//可变短路器第二波节点位置
        [SerializeField] InputField VariableWavelengthInShortCircuit;//可变短路器中波长 
        [SerializeField] InputField OpenLoadPosition;//开路负载位置
        [SerializeField] InputField WaveNodePosShortCircuit;//波节点位置短路
        [SerializeField] InputField WaveNodePosShortTerminal;//波节点位置终端
        [SerializeField] InputField WaveNodePosShortMatching;//波节点位置匹配
        [SerializeField] InputField PhaseAngleCircuit;//相角短路
        [SerializeField] InputField PhaseAngleTerminal;//相角终端
        [SerializeField] InputField PhaseAngleMatching;//相角匹配
        [SerializeField] InputField StandingWaveRatioCircuit;//驻波比短路
        [SerializeField] InputField StandingWaveRatioTerminal;//驻波比终端
        [SerializeField] InputField StandingWaveRatioMatching;//驻波比匹配
        [SerializeField] InputField inputΓ1S;
        [SerializeField] InputField inputΓ10;
        [SerializeField] InputField inputΓ1L;
        [SerializeField] InputField ReflectionCoefficientΓ1SReal;//反射系数短路
        [SerializeField] InputField ReflectionCoefficientΓ10Real;//反射系数终端
        [SerializeField] InputField ReflectionCoefficientΓ1LReal;//反射系数匹配
        [SerializeField] InputField ReflectionCoefficientΓ1SImaginary;//反射系数短路
        [SerializeField] InputField ReflectionCoefficientΓ10Imaginary;//反射系数终端
        [SerializeField] InputField ReflectionCoefficientΓ1LImaginary;//反射系数匹配
        [SerializeField] InputField inputS11Real;
        [SerializeField] InputField inputS11Imaginary;
        [SerializeField] InputField inputS12S21Real;
        [SerializeField] InputField inputS12S21Imaginary;
        [SerializeField] InputField inputS22Real;
        [SerializeField] InputField inputS22Imaginary;

        public override void SaveData()
        {
            base.SaveData();
            labReport1Data.SourceFrequency = StringToDouble(SourceFrequency.text);//信号源频率
            labReport1Data.SourceVoltage = StringToDouble(SourceVoltage.text);//信号源电压设置
            labReport1Data.Attenuator = StringToDouble(Attenuator.text);//衰减器设置
            labReport1Data.EquivalentSectionPosition = StringToDouble(EquivalentSectionPosition.text);//等效截面位置
            labReport1Data.InputWavelength = StringToDouble(InputWavelength.text);//输入端波长
            labReport1Data.VariableShortCircuitFirstPos = StringToDouble(VariableShortCircuitFirstPos.text);//可变短路器第一波节点位置
            labReport1Data.VariableShortCircuitSecondPos = StringToDouble(VariableShortCircuitSecondPos.text);//可变短路器第二波节点位置
            labReport1Data.VariableWavelengthInShortCircuit = StringToDouble(VariableWavelengthInShortCircuit.text);//可变短路器中波长 
            labReport1Data.OpenLoadPosition = StringToDouble(OpenLoadPosition.text);//开路负载位置
            labReport1Data.WaveNodePosShortCircuit = StringToDouble(WaveNodePosShortCircuit.text);//波节点位置短路
            labReport1Data.WaveNodePosShortTerminal = StringToDouble(WaveNodePosShortTerminal.text);//波节点位置终端
            labReport1Data.WaveNodePosShortMatching = StringToDouble(WaveNodePosShortMatching.text);//波节点位置匹配
            labReport1Data.PhaseAngleCircuit = StringToDouble(PhaseAngleCircuit.text);//相角短路
            labReport1Data.PhaseAngleTerminal = StringToDouble(PhaseAngleTerminal.text);//相角终端
            labReport1Data.PhaseAngleMatching = StringToDouble(PhaseAngleMatching.text);//相角匹配
            labReport1Data.StandingWaveRatioCircuit = StringToDouble(StandingWaveRatioCircuit.text);//驻波比短路
            labReport1Data.StandingWaveRatioTerminal = StringToDouble(StandingWaveRatioTerminal.text);//驻波比终端
            labReport1Data.StandingWaveRatioMatching = StringToDouble(StandingWaveRatioMatching.text);//驻波比匹配
            labReport1Data.inputΓ1S = StringToDouble(inputΓ1S.text);
            labReport1Data.inputΓ10 = StringToDouble(inputΓ10.text);
            labReport1Data.inputΓ1L = StringToDouble(inputΓ1L.text);
            labReport1Data.ReflectionCoefficientΓ1SReal = StringToDouble(ReflectionCoefficientΓ1SReal.text);//反射系数短路
            labReport1Data.ReflectionCoefficientΓ10Real = StringToDouble(ReflectionCoefficientΓ10Real.text);//反射系数终端
            labReport1Data.ReflectionCoefficientΓ1LReal = StringToDouble(ReflectionCoefficientΓ1LReal.text);//反射系数匹配
            labReport1Data.ReflectionCoefficientΓ1SImaginary = StringToDouble(ReflectionCoefficientΓ1SImaginary.text);//反射系数短路
            labReport1Data.ReflectionCoefficientΓ10Imaginary = StringToDouble(ReflectionCoefficientΓ10Imaginary.text);//反射系数终端
            labReport1Data.ReflectionCoefficientΓ1LImaginary = StringToDouble(ReflectionCoefficientΓ1LImaginary.text);//反射系数匹配
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
            Attenuator.text = (1 - MathTool.δ).ToString("#0.00");
            SourceFrequency.readOnly = true;
            SourceVoltage.readOnly = true;
            Attenuator.readOnly = true;
        }
    }
}