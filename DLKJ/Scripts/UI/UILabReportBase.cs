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
    public double SourceFrequency;//�ź�ԴƵ��
    public double SourceVoltage;//�ź�Դ��ѹ����
    public double Attenuator;//˥��������
    public double EquivalentSectionPosition;//��Ч����λ��
    public double InputWavelength;//����˲���
    public double VariableShortCircuitFirstPos;//�ɱ��·����һ���ڵ�λ��
    public double VariableShortCircuitSecondPos;//�ɱ��·���ڶ����ڵ�λ��
    public double VariableWavelengthInShortCircuit;//�ɱ��·���в��� 
    public double OpenLoadPosition;//��·����λ��
    public double WaveNodePosShortCircuit;//���ڵ�λ�ö�·
    public double WaveNodePosShortTerminal;//���ڵ�λ���ն�
    public double WaveNodePosShortMatching;//���ڵ�λ��ƥ��
    public double PhaseAngleCircuit;//��Ƕ�·
    public double PhaseAngleTerminal;//����ն�
    public double PhaseAngleMatching;//���ƥ��
    public double StandingWaveRatioCircuit;//פ���ȶ�·
    public double StandingWaveRatioTerminal;//פ�����ն�
    public double StandingWaveRatioMatching;//פ����ƥ��
    public double input��1S;
    public double input��10;
    public double input��1L;
    public double ReflectionCoefficient��1S;//����ϵ����·
    public double ReflectionCoefficient��10;//����ϵ���ն�
    public double ReflectionCoefficient��1L;//����ϵ��ƥ��
    public double inputS11;
    public double inputS12S21;
    public double inputS22;
}
public class LabReportCorrect1Data : LabReportData
{
    public double SourceFrequency;//�ź�ԴƵ��
    public double SourceVoltage;//�ź�Դ��ѹ����
    public double Attenuator;//˥��������
    public double EquivalentSectionPosition;//��Ч����λ��
    public double InputWavelength;//����˲���
    public List<double> VariableShortCircuitFirstPos;//�ɱ��·����һ���ڵ�λ��
    public List<double> VariableShortCircuitSecondPos;//�ɱ��·���ڶ����ڵ�λ��
    public double VariableWavelengthInShortCircuit;//�ɱ��·���в��� 
    public List<double> OpenLoadPosition;//��·����λ��
    public double WaveNodePosShortCircuit;//���ڵ�λ�ö�·
    public double WaveNodePosShortTerminal;//���ڵ�λ���ն�
    public double WaveNodePosShortMatching;//���ڵ�λ��ƥ��
    public double PhaseAngleCircuit;//��Ƕ�·
    public double PhaseAngleTerminal;//����ն�
    public double PhaseAngleMatching;//���ƥ��
    public double StandingWaveRatioCircuit;//פ���ȶ�·
    public double StandingWaveRatioTerminal;//פ�����ն�
    public double StandingWaveRatioMatching;//פ����ƥ��
    public double input��1S;
    public double input��10;
    public double input��1L;
    public double ReflectionCoefficient��1S;//����ϵ����·
    public double ReflectionCoefficient��10;//����ϵ���ն�
    public double ReflectionCoefficient��1L;//����ϵ��ƥ��
    public double inputS11;
    public double inputS12S21;
    public double inputS22;
}
public class LabReport2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//�ź�ԴƵ��
    public double inputSourceVoltageFirst;//�ź�Դ��ѹ
    public double inputAttenuatorSetupFirst;//˥��������
    public double SWRFirst;//פ����
    public double WaveguideWavelengthFirst;//��������
    public double EquivalentSectionPositionFirst;//��Ч����λ��
    public double WaveNodePositionFirst;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceFirst;//��һ�������迹
    public double LoadImpedanceFirst;//�����迹
    public double ScrewPositionFirst;//�ݶ�λ��
    public double ScrewDepthFirst;//�ݶ����
    public double MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ
    public double MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ
    public double SWRAfterMatchingFirst;//ƥ���פ����
    public double inputSourceFrequencySecond;//�ź�ԴƵ��
    public double inputSourceVoltageSecond;//�ź�Դ��ѹ
    public double inputAttenuatorSetupSecond;//˥��������
    public double SWRSecond;//פ����
    public double WaveguideWavelengthSecond;//��������
    public double EquivalentSectionPositionSecond;//��Ч����λ��
    public double WaveNodePositionSecond;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceSecond;//��һ�������迹
    public double LoadImpedanceSecond;//�����迹
    public double ScrewPositionSecond;//�ݶ�λ��
    public double ScrewDepthSecond;//�ݶ����
    public double MinimumVoltageAfterMatchingSecond;//ƥ�����С��ѹ
    public double MaximumVoltageAfterMatchingSecond;//ƥ�������ѹ
    public double SWRAfterMatchingSecond;//ƥ���פ����
}
public class LabReportCorrect2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//�ź�ԴƵ��
    public double inputSourceVoltageFirst;//�ź�Դ��ѹ
    public double inputAttenuatorSetupFirst;//˥��������
    public double SWRFirst;//פ����
    public double WaveguideWavelengthFirst;//��������
    public double EquivalentSectionPositionFirst;//��Ч����λ��
    public double WaveNodePositionFirst;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceFirst;//��һ�������迹
    public double LoadImpedanceFirst;//�����迹
    public List<double> ScrewPositionFirst;//�ݶ�λ��
    public List<double> ScrewDepthFirst;//�ݶ����
    public double MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ
    public double MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ
    public double SWRAfterMatchingFirst;//ƥ���פ����
    public double inputSourceFrequencySecond;//�ź�ԴƵ��
    public double inputSourceVoltageSecond;//�ź�Դ��ѹ
    public double inputAttenuatorSetupSecond;//˥��������
    public double SWRSecond;//פ����
    public double WaveguideWavelengthSecond;//��������
    public double EquivalentSectionPositionSecond;//��Ч����λ��
    public double WaveNodePositionSecond;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceSecond;//��һ�������迹
    public double LoadImpedanceSecond;//�����迹
    public List<double> ScrewPositionSecond;//�ݶ�λ��
    public List<double> ScrewDepthSecond;//�ݶ����
    public double MinimumVoltageAfterMatchingSecond;//ƥ�����С��ѹ
    public double MaximumVoltageAfterMatchingSecond;//ƥ�������ѹ
    public double SWRAfterMatchingSecond;//ƥ���פ����
}

public class LabReport3Data : LabReportData
{
    public double OnePortVoltage;//һ�˿ڵ�ѹ
    public double ThreePortVoltage;//���˿ڵ�ѹ
    public double CouplingFactor;//��϶�C
}

public class UILabReportBase : MonoBehaviour
{
    private InputField[] inputFields;
    public string filePath;
    public string outFilePath;
    protected Dictionary<string, object> map = new Dictionary<string, object>();
    [Header("ʵ������")] public string ExperimentItem;
    [Header("ʵ������")] public string ExperimentType;
    [Header("ʵ��ѧʱ")] public string ExperimentTime;
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
        bool isInput = true;//�Ƿ�������
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
        if (isInput == false)
        {
            for (int i = 0; i < cacheEffect.Count; i++)
            {
                cacheEffect[i].StartFlashing();
            }
            OpenSecondPage();
        }
        return isInput;
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
    private void OpenSecondPage()
    {
        SetVisibale(true);
        ChangePageButton.transform.localEulerAngles = new Vector3(0, 0, 180);
        page1.SetActive(false);
        page2.SetActive(true);
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
    /// ���ݸ�ʽ����
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

    /// <summary>
    /// �˶���ӵĴ��Ƿ���ȷ
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
            Debug.Log("����ȷ" + inputValue + rightAnswer);
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
        ProxyManager.saveProxy.Save();
        WordHelper.HandleGuaranteeDoc(filePath, map, outFilePath);
    }
}
