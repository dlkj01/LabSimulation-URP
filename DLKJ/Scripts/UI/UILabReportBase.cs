using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;
using System.Linq;
using DLKJ;
using Common;

public struct UserDate
{
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
    public double ReflectionCoefficientΓ1S;//反射系数短路
    public double ReflectionCoefficientΓ10;//反射系数终端
    public double ReflectionCoefficientΓ1L;//反射系数匹配
    public double inputS11;
    public double inputS12S21;
    public double inputS22;
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
    public double inputΓ1S;
    public double inputΓ10;
    public double inputΓ1L;
    public double ReflectionCoefficientΓ1S;//反射系数短路
    public double ReflectionCoefficientΓ10;//反射系数终端
    public double ReflectionCoefficientΓ1L;//反射系数匹配
    public double inputS11;
    public double inputS12S21;
    public double inputS22;
}
public class LabReport2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//信号源频率
    public double inputSourceVoltageFirst;//信号源电压
    public double inputAttenuatorSetupFirst;//衰减器设置
    public double SWRFirst;//驻波比
    public double WaveguideWavelengthFirst;//波导波长
    public double EquivalentSectionPositionFirst;//等效截面位置
    public double WaveNodePositionFirst;//第一波节点位置
    public double NormalizedLoadImpedanceFirst;//归一化负载阻抗
    public double LoadImpedanceFirst;//负载阻抗
    public double ScrewPositionFirst;//螺钉位置
    public double ScrewDepthFirst;//螺钉深度
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
    public double ScrewPositionSecond;//螺钉位置
    public double ScrewDepthSecond;//螺钉深度
    public double MinimumVoltageAfterMatchingSecond;//匹配后最小电压
    public double MaximumVoltageAfterMatchingSecond;//匹配后最大电压
    public double SWRAfterMatchingSecond;//匹配后驻波比
}
public class LabReportCorrect2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//信号源频率
    public double inputSourceVoltageFirst;//信号源电压
    public double inputAttenuatorSetupFirst;//衰减器设置
    public double SWRFirst;//驻波比
    public double WaveguideWavelengthFirst;//波导波长
    public double EquivalentSectionPositionFirst;//等效截面位置
    public double WaveNodePositionFirst;//第一波节点位置
    public double NormalizedLoadImpedanceFirst;//归一化负载阻抗
    public double LoadImpedanceFirst;//负载阻抗
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
    public double OnePortVoltage;//一端口电压
    public double ThreePortVoltage;//三端口电压
    public double CouplingFactor;//耦合度C
}

public class UILabReportBase : MonoBehaviour
{
    private InputField[] inputFields;
    public string filePath;
    public string outFilePath;
    protected Dictionary<string, object> map = new Dictionary<string, object>();
    [Header("实验名称")] public string ExperimentItem;
    [Header("实验类型")] public string ExperimentType;
    [Header("实验学时")] public string ExperimentTime;
    [SerializeField] GameObject SaveButton;
    [SerializeField] GameObject CloseButton;
    [SerializeField] GameObject ChangePageButton;
    [SerializeField] Text ExperimentItemText;
    [SerializeField] Text ExperimentTypeText;
    [SerializeField] Text ExperimentTimeText;
    [SerializeField] protected InputField nameInputField;
    [SerializeField] protected InputField classInputField;
    [SerializeField] protected InputField timeInputField;
    [SerializeField] protected InputField idInputField;
    [SerializeField] protected InputField teacherInputField;
    public UserDate userData = new UserDate();
    [SerializeField] GameObject page1, page2;
    protected CanvasGroup canvasGroup;
    private Coroutine coroutine;
    bool isPlaying = false;
    private float fadeSpeed = 5;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        ExperimentItemText.text = ExperimentItem;
        ExperimentTypeText.text = ExperimentType;
        ExperimentTimeText.text = ExperimentTime;
        UIEventListener.GetUIEventListener(SaveButton).PointerClick += (P) => { SaveData(); };
        UIEventListener.GetUIEventListener(CloseButton).PointerClick += (P) => { SetVisibale(false); };
        UIEventListener.GetUIEventListener(ChangePageButton).PointerClick += (P) =>
        {
            ChangePage(P);
        };
        inputFields = GetComponentsInChildren<InputField>(true);
        foreach (var item in inputFields)
        {
            UIEffect uiEffect = item.gameObject.AddComponent<UIEffect>();
        }
        page2.SetActive(true);
        page2.SetActive(false);
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

    public void ShowPanle(bool isInput, bool secondPage = true)
    {
        if (isInput == false)
        {
            for (int i = 0; i < cacheEffect.Count; i++)
            {
                cacheEffect[i].StartFlashing();
            }
            OpenSecondPage(secondPage);
        }
    }
    public void SetVisibale(bool state)
    {
        canvasGroup.alpha = state == false ? 0 : 1;
        canvasGroup.blocksRaycasts = state;
    }
    public void ChangePage(PointerEventData data)
    {
        bool state = page1.activeSelf;
        if (state == true)
            data.pointerPress.transform.localEulerAngles = new Vector3(0, 0, 180);
        else
            data.pointerPress.transform.localEulerAngles = new Vector3(0, 0, 0);
        page1.SetActive(!state);
        page2.SetActive(state);
    }
    private void OpenSecondPage(bool secondPage = true)
    {
        SetVisibale(true);
        ChangePageButton.transform.localEulerAngles = new Vector3(0, 0, 180);
        if (secondPage)
        {
            page1.SetActive(false);
            page2.SetActive(true);
        }
        else
        {
            page1.SetActive(true);
            page2.SetActive(false);
        }
    }

    public virtual void SaveData()
    {
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
        double result = -9999;
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
        double lerp = rightAnswer * 0.2f;
        bool result = inputValue < rightAnswer + lerp && inputValue > rightAnswer - lerp;
        if (result)
        {
            string labName = SceneManager.GetInstance().GetCurrentLabName();
            switch (labName)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 27f * 0.4f;
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 30f * 0.5f;
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 3f * 0.1f;
                    break;
                default:
                    SceneManager.GetInstance().currentLabScore = 0;
                    break;
            }
            // MathTool.score += 1.724f;
            Debug.Log("答案正确" + inputValue + rightAnswer);
        }
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
                        result = -9999;
                    }
                    else
                    {
                        result = (double)item2.Value;
                    }
                    answerCheck.isRight = DataFormatParsing(result, fields[i].GetValue(rightAnswer));
                }
            }
            this.map[item2.Key] = answerCheck;
        }

        WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
}
