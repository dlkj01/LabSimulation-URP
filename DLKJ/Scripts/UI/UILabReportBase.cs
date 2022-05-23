using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using DLKJ;

public struct UserDate
{
    public string schoolTitle;
    public string userName;
    public string className;
    public string time;
    public string id;
    public string teacherName;
}
public class LabReportData { }
public class LabReport1Data : LabReportData
{
    public double SourceFrequency;//信号源频率
    public double SourceVoltage;//信号源电压设置
    public double Attenuator;//衰减器设置
    public double EquivalentSectionPosition;//等效截面位置
    public double InputWavelength;//输入端波长
    public double VariableShortCircuitFirstPos;//可变短路器第一波节点位置
    public double VariableShortCircuitSecondPos;//可变短路器第二波节点位置
    public double VariableWavelengthInShortCircuit;//可变短路器中波长 
    public double OpenLoadPosition;//开路负载位置
    public double WaveNodePosShortCircuit;//波节点位置短路
    public double WaveNodePosShortTerminal;//波节点位置终端
    public double WaveNodePosShortMatching;//波节点位置匹配
    public double PhaseAngleCircuit;//相角短路
    public double PhaseAngleTerminal;//相角终端
    public double PhaseAngleMatching;//相角匹配
    public double StandingWaveRatioCircuit;//驻波比短路
    public double StandingWaveRatioTerminal;//驻波比终端
    public double StandingWaveRatioMatching;//驻波比匹配
    public double inputΓ1S;
    public double inputΓ10;
    public double inputΓ1L;
    public double ReflectionCoefficientΓ1SReal;
    public double ReflectionCoefficientΓ10Real;
    public double ReflectionCoefficientΓ1LReal;
    public double ReflectionCoefficientΓ1SImaginary;
    public double ReflectionCoefficientΓ10Imaginary;
    public double ReflectionCoefficientΓ1LImaginary;
    public double inputS11Real;
    public double inputS11Imaginary;
    public double inputS12S21Real;
    public double inputS12S21Imaginary;
    public double inputS22Real;
    public double inputS22Imaginary;
}
public class LabReportCorrect1Data : LabReportData
{
    public double SourceFrequency;//信号源频率
    public double SourceVoltage;//信号源电压设置
    public double Attenuator;//衰减器设置
    public double EquivalentSectionPosition;//等效截面位置
    public double InputWavelength;//输入端波长
    public List<double> VariableShortCircuitFirstPos;//可变短路器第一波节点位置
    public List<double> VariableShortCircuitSecondPos;//可变短路器第二波节点位置
    public double VariableWavelengthInShortCircuit;//可变短路器中波长 
    public List<double> OpenLoadPosition;//开路负载位置
    public double WaveNodePosShortCircuit;//波节点位置短路
    public double WaveNodePosShortTerminal;//波节点位置终端
    public double WaveNodePosShortMatching;//波节点位置匹配
    public double PhaseAngleCircuit;//相角短路
    public double PhaseAngleTerminal;//相角终端
    public double PhaseAngleMatching;//相角匹配
    public double StandingWaveRatioCircuit;//驻波比短路
    public double StandingWaveRatioTerminal;//驻波比终端
    public double StandingWaveRatioMatching;//驻波比匹配
    public double inputΓ1S;                 //短路反射系数
    public double inputΓ10;                 //开路反射系数
    public double inputΓ1L;                 //匹配反射系数
    public double ReflectionCoefficientΓ1SReal;     //短路系数实部
    public double ReflectionCoefficientΓ10Real;
    public double ReflectionCoefficientΓ1LReal;
    public double ReflectionCoefficientΓ1SImaginary;
    public double ReflectionCoefficientΓ10Imaginary;
    public double ReflectionCoefficientΓ1LImaginary;
    public double inputS11Real;
    public double inputS11Imaginary;
    public double inputS12S21Real;
    public double inputS12S21Imaginary;
    public double inputS22Real;
    public double inputS22Imaginary;
}
public class LabReport2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//信号源频率
    public double inputSourceVoltageFirst;//信号源电压
    public double inputAttenuatorSetupFirst;//衰减器设置
    public double SWRFirst;//驻波比
    public double WaveguideWavelengthFirst;//波导波长
    public double EquivalentSectionPositionFirst;//等效截面位置
    public double MinimumVoltage;
    public double MaximumVoltage;
    public double WaveNodePositionFirst;//第一波节点位置
    public double NormalizedLoadImpedanceFirstReal;//归一化负载阻抗
    public double NormalizedLoadImpedanceFirstImaginary;
    public double LoadImpedanceFirstReal;//负载阻抗实部
    public double LoadImpedanceFirstImaginary;//负载阻抗虚部
    public double ScrewPositionFirst;//螺钉位置
    public double ScrewDepthFirst;//螺钉深度
    public double MinimumVoltageAfterMatchingFirst;//匹配后最小电压
    public double MaximumVoltageAfterMatchingFirst;//匹配后最大电压
    public double SWRAfterMatchingFirst;//匹配后驻波比
}
public class LabReportCorrect2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//信号源频率
    public double inputSourceVoltageFirst;//信号源电压
    public double inputAttenuatorSetupFirst;//衰减器设置
    public double SWRFirst;//驻波比
    public double WaveguideWavelengthFirst;//波导波长
    public double EquivalentSectionPositionFirst;//等效截面位置
    public double MinimumVoltage;           //最小电压
    public double MaximumVoltage;           //最大电压
    public double WaveNodePositionFirst;//第一波节点位置
    public double NormalizedLoadImpedanceFirstReal;//归一化负载阻抗
    public double NormalizedLoadImpedanceFirstImaginary;
    public double LoadImpedanceFirstReal;//负载阻抗实部
    public double LoadImpedanceFirstImaginary;//负载阻抗虚部
    public List<double> ScrewPositionFirst;//螺钉位置
    public List<double> ScrewDepthFirst;//螺钉深度
    public double MinimumVoltageAfterMatchingFirst;//匹配后最小电压
    public double MaximumVoltageAfterMatchingFirst;//匹配后最大电压
    public double SWRAfterMatchingFirst;//匹配后驻波比
    public double inputSourceFrequencySecond;//信号源频率
    public double inputSourceVoltageSecond;//信号源电压
    public double inputAttenuatorSetupSecond;//衰减器设置
    public double SWRSecond;//驻波比
    public double WaveguideWavelengthSecond;//波导波长
    public double EquivalentSectionPositionSecond;//等效截面位置
    public double WaveNodePositionSecond;//第一波节点位置
    public double NormalizedLoadImpedanceSecond;//归一化负载阻抗
    public double LoadImpedanceSecond;//负载阻抗
    public List<double> ScrewPositionSecond;//螺钉位置
    public List<double> ScrewDepthSecond;//螺钉深度
    public double MinimumVoltageAfterMatchingSecond;//匹配后最小电压
    public double MaximumVoltageAfterMatchingSecond;//匹配后最大电压
    public double SWRAfterMatchingSecond;//匹配后驻波比
}

public class LabReport3Data : LabReportData
{
    public double inputSourceFrequency;
    public double inputSourceVoltage;
    public double OnePortVoltage;//一端口电压
    public double ThreePortVoltage;//三端口电压
    public double CouplingFactor;//耦合度C
}

public class UILabReportBase : MonoBehaviour
{
    public float GetHeight
    {
        get
        {
            float height = 0;
            switch (SceneManager.GetInstance().GetCurrentLabName())
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    height = 6850f;
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    height = 5664f;
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    height = 3520f;
                    break;
                default:
                    break;
            }
            return height;
        }
    }
    public virtual void SetInputTextReadOnly() { }
    private InputField[] inputFields;
    public string filePath;
    public string outFilePath;
    public Dictionary<string, object> map = new Dictionary<string, object>();
    [SerializeField] GameObject CloseButton;
    [SerializeField] protected InputField schoolInputField;
    [SerializeField] protected InputField nameInputField;
    [SerializeField] protected InputField classInputField;
    [SerializeField] protected InputField timeInputField;
    [SerializeField] protected InputField idInputField;
    [SerializeField] protected InputField teacherInputField;
    public UserDate userData = new UserDate();
    protected CanvasGroup canvasGroup;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        UIEventListener.GetUIEventListener(CloseButton).PointerClick += (P) => { SetVisibale(false); };
        inputFields = GetComponentsInChildren<InputField>(true);
        foreach (var item in inputFields)
        {
            UIEffect uiEffect = item.gameObject.AddComponent<UIEffect>();
        }
    }
    List<UIEffect> cacheEffect = new List<UIEffect>();
    public bool FinishedStepInput(string[] inputFieldName)
    {
        for (int i = 0; i < cacheEffect.Count; i++)
        {
            cacheEffect[i].StopFlashing();
        }
        cacheEffect.Clear();
        bool isInput = true;//是否有输入
        foreach (var item in inputFieldName)
        {
            for (int i = 0; i < inputFields.Length; i++)
            {
                if (inputFields[i].gameObject.name == item)
                {
                    if (string.IsNullOrEmpty(inputFields[i].text))
                    {
                        Debug.Log(inputFields[i].gameObject.name);
                        isInput = false;
                        cacheEffect.Add(inputFields[i].GetComponent<UIEffect>());
                    }
                }
            }
        }
        return isInput;
    }
    public RectTransform ContentTF;//内容变换组件
    private void SetContentPosition(float height)
    {
        ContentTF.anchoredPosition = new Vector2(0, height);
    }
    public int GetPagesLenght { get { return ContentTF.childCount; } }
    public void ShowFlashingImage(float height)
    {
        for (int i = 0; i < cacheEffect.Count; i++)
        {
            cacheEffect[i].StartFlashing();
        }
        SetVisibale(true, height);
    }
    /// <summary>
    /// 设置页面开启关闭。
    /// </summary>
    /// <param name="state">开关</param>
    /// <param name="init">是否为初始值,如果为true，默认打开第一页</param>
    /// <param name="index">翻到第几页</param>
    public void SetVisibale(bool state, float height = 0)
    {
        SetContentPosition(height);
        canvasGroup.alpha = state == false ? 0 : 1;
        canvasGroup.blocksRaycasts = state;
    }


    public virtual void SaveData()
    {
        userData.schoolTitle = schoolInputField.text;
        userData.userName = nameInputField.text;
        userData.className = classInputField.text;
        userData.time = timeInputField.text;
        userData.id = idInputField.text;
        userData.teacherName = teacherInputField.text;
        Dictionary<string, object> map1 = WordHelper.GetFields(userData);
        foreach (var item in map1)
            map[item.Key] = item.Value;
    }
    protected double StringToDouble(string value)
    {
        double result = -99999999;
        if (string.IsNullOrEmpty(value))
        {
            return result;
        }
        double.TryParse(value, out result);
        return result;
    }

    /// <summary>
    /// 数据格式解析
    /// </summary>
    /// <param name="inputValue"></param>
    /// <param name="rightAnswer"></param>
    /// <returns></returns>
    protected bool DataFormatParsing(double inputValue, object rightAnswer)
    {
        if (rightAnswer is double d)
        {
            return VerifyScore(inputValue, d);
        }
        if (rightAnswer is List<double> a)
        {
            for (int i = 0; i < a.Count; i++)
            {
                if (VerifyScore(inputValue, a[i]))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public string GetInputValue(string inputTextName)
    {
        foreach (var item in inputFields)
        {
            if (item.name == inputTextName)
            {
                return item.text;
            }
        }
        return "";
    }

    /// <summary>
    /// 核对添加的答案是否正确
    /// </summary>
    /// <param name="inputValue"></param>
    /// <param name="rightAnswer"></param>
    /// <returns></returns>
    protected bool VerifyScore(double inputValue, double rightAnswer)
    {
        Debug.Log("rightAnswer:" + rightAnswer+ "    inputValue:" + inputValue);
        double lerp = rightAnswer * 0.2f;
        bool result = inputValue < rightAnswer + lerp && inputValue > rightAnswer - lerp;
        if (result)
        {
            string labName = SceneManager.GetInstance().GetCurrentLabName();
            switch (labName)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 30f;
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 15f;
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 5f;
                    break;
            }
            Debug.Log("答案正确" + inputValue + rightAnswer);
        }
        Debug.Log("当前实验得分：" + SceneManager.GetInstance().currentLabScore);
        return result;
    }

    protected void AddResult(LabReportData labReportData, LabReportData rightAnswer)
    {
        Dictionary<string, object> addMap = WordHelper.GetFields(labReportData);
        foreach (var item2 in addMap)
        {
            AnswerCheck answerCheck = new AnswerCheck();
            answerCheck.answer = item2.Value.ToString();
            System.Reflection.FieldInfo[] fields = rightAnswer.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name == item2.Key)
                {
                    double result;
                    if (String.IsNullOrEmpty(item2.Value.ToString()))
                    {
                        result = -99999999;
                    }
                    else
                    {
                        result = double.Parse(item2.Value.ToString());
                    }
                    //switch (item2.Key)
                    //{
                    //    case "WaveguideWavelengthFirst":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "EquivalentSectionPositionFirst":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "ScrewPositionFirst":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "EquivalentSectionPosition":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "InputWavelength":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "VariableShortCircuitFirstPos":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "VariableWavelengthInShortCircuit":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "VariableShortCircuitSecondPos":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "OpenLoadPosition":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "WaveNodePositionFirst":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "WaveNodePosShortCircuit":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "WaveNodePosShortTerminal":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;
                    //    case "WaveNodePosShortMatching":
                    //        {
                    //            result = result * 0.001f;
                    //        }
                    //        break;

                    //    default:
                    //        break;
                    //}
                    if (item2.Key == "SourceFrequency" || item2.Key == "SourceVoltage" || item2.Key == "Attenuator"
                        || item2.Key == "inputSourceFrequencyFirst" || item2.Key == "inputSourceVoltageFirst" || item2.Key == "inputAttenuatorSetupFirst"
                        || item2.Key == "inputSourceFrequency" || item2.Key == "inputSourceVoltage")
                    {
                        answerCheck.isRight = true;
                        continue;
                    }
                    answerCheck.isRight = DataFormatParsing(result, fields[i].GetValue(rightAnswer));
                }
            }
            this.map[item2.Key] = answerCheck;
        }
        WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }

}
