using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

public struct LabReport2Data
{
    public double inputSourceFrequencyFirst;//�ź�ԴƵ��
    public double inputSourceVoltageFirst;//�ź�Դ��ѹ
    public double inputAttenuatorSetupFirst;//˥��������
    public double SWRFirst;//פ����
    public double WaveguideWavelengthFirst;//��������
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
    public double WaveNodePositionSecond;//��һ���ڵ�λ��
    public double NormalizedLoadImpedanceSecond;//��һ�������迹
    public double LoadImpedanceSecond;//�����迹
    public double ScrewPositionSecond;//�ݶ�λ��
    public double ScrewDepthSecond;//�ݶ����
    public double MinimumVoltageAfterMatchingSecond;//ƥ�����С��ѹ
    public double MaximumVoltageAfterMatchingSecond;//ƥ�������ѹ
    public double SWRAfterMatchingSecond;//ƥ���פ����
}

public struct LabReport3Data
{
    public double OnePortVoltage;//һ�˿ڵ�ѹ
    public double ThreePortVoltage;//���˿ڵ�ѹ
    public double CouplingFactor;//��϶�C
}

public class UILabReportBase : MonoBehaviour
{
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


    protected virtual void SaveData()
    {
        userData.userName = nameInputField.text;
        userData.className = classInputField.text;
        userData.time = timeInputField.text;
        userData.id = idInputField.text;
        userData.teacherName = teacherInputField.text;
    }
    protected double StringToDouble(string value)
    {
        double result = 0;
        if (string.IsNullOrEmpty(value))
        {
            return result;
        }
        double.TryParse(value, out result);
        return result;
    }
}
