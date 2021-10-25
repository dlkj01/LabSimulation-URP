using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;
using System.Linq;

public struct UserDate
{
    public string userName;
    public string className;
    public string time;
    public string id;
    public string teacherName;
}
public struct LabReport1Data
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
public struct LabReportCorrect1Data
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
public struct LabReport2Data
{
    public double inputSourceFrequencyFirst;//信号源频率
    public double inputSourceVoltageFirst;//信号源电压
    public double inputAttenuatorSetupFirst;//衰减器设置
    public double SWRFirst;//驻波比
    public double WaveguideWavelengthFirst;//波导波长
    public double EquivalentSectionPosition;//等效截面位置
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
    public double WaveNodePositionSecond;//第一波节点位置
    public double NormalizedLoadImpedanceSecond;//归一化负载阻抗
    public double LoadImpedanceSecond;//负载阻抗
    public double ScrewPositionSecond;//螺钉位置
    public double ScrewDepthSecond;//螺钉深度
    public double MinimumVoltageAfterMatchingSecond;//匹配后最小电压
    public double MaximumVoltageAfterMatchingSecond;//匹配后最大电压
    public double SWRAfterMatchingSecond;//匹配后驻波比
}
public struct LabReportCorrect2Data
{
    public double inputSourceFrequencyFirst;//信号源频率
    public double inputSourceVoltageFirst;//信号源电压
    public double inputAttenuatorSetupFirst;//衰减器设置
    public double SWRFirst;//驻波比
    public double WaveguideWavelengthFirst;//波导波长
    public double EquivalentSectionPosition;//等效截面位置
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
    public double WaveNodePositionSecond;//第一波节点位置
    public double NormalizedLoadImpedanceSecond;//归一化负载阻抗
    public double LoadImpedanceSecond;//负载阻抗
    public double ScrewPositionSecond;//螺钉位置
    public double ScrewDepthSecond;//螺钉深度
    public double MinimumVoltageAfterMatchingSecond;//匹配后最小电压
    public double MaximumVoltageAfterMatchingSecond;//匹配后最大电压
    public double SWRAfterMatchingSecond;//匹配后驻波比
}

public struct LabReport3Data
{
    public double OnePortVoltage;//一端口电压
    public double ThreePortVoltage;//三端口电压
    public double CouplingFactor;//耦合度C
}

public class UILabReportBase : MonoBehaviour
{
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
    [SerializeField] InputField classInputField;
    [SerializeField] InputField timeInputField;
    [SerializeField] InputField idInputField;
    [SerializeField] InputField teacherInputField;
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
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
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

    }
    public void SetVisibale(bool state)
    {
        if (canvasGroup.alpha == 0 && state == false) return;
        if (canvasGroup.alpha == 1 && state == true) return;
        if (isPlaying == true)
            return;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(SetVisibleDelay(state));
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
    IEnumerator SetVisibleDelay(bool state)
    {
        canvasGroup.alpha = state == false ? 1 : 0;
        isPlaying = true;
        if (state == true)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }
            isPlaying = false;
        }
        else
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
            isPlaying = false;
        }
        if (canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }


    public virtual void SaveData()
    {
        userData.userName = nameInputField.text;
        userData.className = classInputField.text;
        userData.time = timeInputField.text;
        userData.id = idInputField.text;
        userData.teacherName = teacherInputField.text;
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



}
