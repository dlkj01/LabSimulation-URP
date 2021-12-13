using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using DLKJ;

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
    public double ReflectionCoefficient��1SReal;
    public double ReflectionCoefficient��10Real;
    public double ReflectionCoefficient��1LReal;
    public double ReflectionCoefficient��1SImaginary;
    public double ReflectionCoefficient��10Imaginary;
    public double ReflectionCoefficient��1LImaginary;
    public double inputS11Real;
    public double inputS11Imaginary;
    public double inputS12S21Real;
    public double inputS12S21Imaginary;
    public double inputS22Real;
    public double inputS22Imaginary;
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
    public double ReflectionCoefficient��1SReal;
    public double ReflectionCoefficient��10Real;
    public double ReflectionCoefficient��1LReal;
    public double ReflectionCoefficient��1SImaginary;
    public double ReflectionCoefficient��10Imaginary;
    public double ReflectionCoefficient��1LImaginary;
    public double inputS11Real;
    public double inputS11Imaginary;
    public double inputS12S21Real;
    public double inputS12S21Imaginary;
    public double inputS22Real;
    public double inputS22Imaginary;
}
public class LabReport2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//�ź�ԴƵ��
    public double inputSourceVoltageFirst;//�ź�Դ��ѹ
    public double inputAttenuatorSetupFirst;//˥��������
    public double SWRFirst;//פ����
    public double WaveguideWavelengthFirst;//��������
    public double EquivalentSectionPositionFirst;//��Ч����λ��
    public double MinimumVoltage;
    public double MaximumVoltage;
    public double WaveNodePositionFirst;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceFirstReal;//��һ�������迹
    public double NormalizedLoadImpedanceFirstImaginary;
    public double LoadImpedanceFirstReal;//�����迹ʵ��
    public double LoadImpedanceFirstImaginary;//�����迹�鲿
    public double ScrewPositionFirst;//�ݶ�λ��
    public double ScrewDepthFirst;//�ݶ����
    public double MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ
    public double MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ
    public double SWRAfterMatchingFirst;//ƥ���פ����
}
public class LabReportCorrect2Data : LabReportData
{
    public double inputSourceFrequencyFirst;//�ź�ԴƵ��
    public double inputSourceVoltageFirst;//�ź�Դ��ѹ
    public double inputAttenuatorSetupFirst;//˥��������
    public double SWRFirst;//פ����
    public double WaveguideWavelengthFirst;//��������
    public double EquivalentSectionPositionFirst;//��Ч����λ��
    public double MinimumVoltage;
    public double MaximumVoltage;
    public double WaveNodePositionFirst;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceFirstReal;//��һ�������迹
    public double NormalizedLoadImpedanceFirstImaginary;
    public double LoadImpedanceFirstReal;//�����迹ʵ��
    public double LoadImpedanceFirstImaginary;//�����迹�鲿
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
    public double inputSourceFrequency;
    public double inputSourceVoltage;
    public double OnePortVoltage;//һ�˿ڵ�ѹ
    public double ThreePortVoltage;//���˿ڵ�ѹ
    public double CouplingFactor;//��϶�C
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
                    height = 5455.81f;
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    height = 4500;
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    height = 2651.801f;
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
        return isInput;
    }
    public RectTransform ContentTF;//���ݱ任���
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
    /// ����ҳ�濪���رա�
    /// </summary>
    /// <param name="state">����</param>
    /// <param name="init">�Ƿ�Ϊ��ʼֵ,���Ϊtrue��Ĭ�ϴ򿪵�һҳ</param>
    /// <param name="index">�����ڼ�ҳ</param>
    public void SetVisibale(bool state, float height = 0)
    {
        SetContentPosition(height);
        canvasGroup.alpha = state == false ? 0 : 1;
        canvasGroup.blocksRaycasts = state;
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
        double result = -99999999;
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
                    SceneManager.GetInstance().currentLabScore += 100f / 30f;
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 15f;
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    SceneManager.GetInstance().currentLabScore += 100f / 5f;
                    break;
            }
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
                        result = -99999999;
                    }
                    else
                    {
                        result = double.Parse(item2.Value.ToString());
                    }
                    switch (item2.Key)
                    {
                        case "WaveguideWavelengthFirst":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "EquivalentSectionPositionFirst":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "ScrewPositionFirst":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "EquivalentSectionPosition":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "InputWavelength":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "VariableShortCircuitFirstPos":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "VariableWavelengthInShortCircuit":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "VariableShortCircuitSecondPos":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "OpenLoadPosition":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "WaveNodePositionFirst":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "WaveNodePosShortCircuit":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "WaveNodePosShortTerminal":
                            {
                                result = result * 0.001f;
                            }
                            break;
                        case "WaveNodePosShortMatching":
                            {
                                result = result * 0.001f;
                            }
                            break;

                        default:
                            break;
                    }
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
