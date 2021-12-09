using DLKJ;
using UnityEngine;
using UnityEngine.UI;
public class UILabReport2 : UILabReportBase
{
    public LabReport2Data labReport2Data = new LabReport2Data();
    [Tooltip("�ź�ԴƵ��")]
    [Header("��һ��------------------------------------------------")]
    public InputField inputSourceFrequencyFirst;//�ź�ԴƵ��

    [Tooltip("�ź�Դ��ѹ")] public InputField inputSourceVoltageFirst;//�ź�Դ��ѹ

    [Tooltip("˥��������")] public InputField inputAttenuatorSetupFirst;//˥��������

    [Tooltip("פ����")] public InputField SWRFirst;//פ����

    [Tooltip("��������")] public InputField WaveguideWavelengthFirst;//��������

    [SerializeField] InputField EquivalentSectionPositionFirst;//��Ч����λ��

    [Tooltip("��С��ѹ")] public InputField MinimumVoltageInput;

    [Tooltip("��ĵ�ѹ")] public InputField MaximumVoltageInput;

    [Tooltip("��һ���ڵ�λ��")] public InputField WaveNodePositionFirst;//��һ���ڵ�λ��

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceFirstReal;//��һ�������迹����

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceFirstImaginary;//��һ�������迹�翹

    [Tooltip("�����迹")] public InputField LoadImpedanceFirstReal;//�����迹����

    [Tooltip("�����迹")] public InputField LoadImpedanceFirstImaginary;//�����迹�翹

    [Tooltip("�ݶ�λ��")] public InputField ScrewPositionFirst;//�ݶ�λ��

    [Tooltip("�ݶ����")] public InputField ScrewDepthFirst;//�ݶ����

    [Tooltip("ƥ�����С��ѹ")] public InputField MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ

    [Tooltip("ƥ�������ѹ")] public InputField MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ

    [Tooltip("ƥ���פ����")] public InputField SWRAfterMatchingFirst;//ƥ���פ����

    public override void SaveData()
    {
        base.SaveData();
        labReport2Data.inputSourceFrequencyFirst = StringToDouble(inputSourceFrequencyFirst.text);
        labReport2Data.inputSourceVoltageFirst = StringToDouble(inputSourceVoltageFirst.text);
        labReport2Data.inputAttenuatorSetupFirst = StringToDouble(inputAttenuatorSetupFirst.text);
        labReport2Data.SWRFirst = StringToDouble(SWRFirst.text);
        labReport2Data.WaveguideWavelengthFirst = StringToDouble(WaveguideWavelengthFirst.text);
        labReport2Data.MinimumVoltage = StringToDouble(MinimumVoltageInput.text);
        labReport2Data.MaximumVoltage = StringToDouble(MaximumVoltageInput.text);
        labReport2Data.EquivalentSectionPositionFirst = StringToDouble(EquivalentSectionPositionFirst.text);
        labReport2Data.WaveNodePositionFirst = StringToDouble(WaveNodePositionFirst.text);
        labReport2Data.NormalizedLoadImpedanceFirstReal = StringToDouble(NormalizedLoadImpedanceFirstReal.text);
        labReport2Data.NormalizedLoadImpedanceFirstImaginary = StringToDouble(NormalizedLoadImpedanceFirstImaginary.text);
        labReport2Data.LoadImpedanceFirstReal = StringToDouble(LoadImpedanceFirstReal.text);
        labReport2Data.LoadImpedanceFirstImaginary = StringToDouble(LoadImpedanceFirstImaginary.text);
        labReport2Data.ScrewPositionFirst = StringToDouble(ScrewPositionFirst.text);
        labReport2Data.ScrewDepthFirst = StringToDouble(ScrewDepthFirst.text);
        labReport2Data.MinimumVoltageAfterMatchingFirst = StringToDouble(MinimumVoltageAfterMatchingFirst.text);
        labReport2Data.MaximumVoltageAfterMatchingFirst = StringToDouble(MaximumVoltageAfterMatchingFirst.text);
        labReport2Data.SWRAfterMatchingFirst = StringToDouble(SWRAfterMatchingFirst.text);
        AddResult(labReport2Data, MathTool.report2CorrectAnswer);
    }

    public override void SetInputTextReadOnly()
    {
        base.SetInputTextReadOnly();
        inputSourceFrequencyFirst.text = MathTool.F.ToString("#0.00");
        inputSourceVoltageFirst.text = MathTool.A.ToString("#0.00");
        inputAttenuatorSetupFirst.text = (1 - MathTool.��).ToString("#0.00");
        inputSourceFrequencyFirst.readOnly = true;
        inputSourceVoltageFirst.readOnly = true;
        inputAttenuatorSetupFirst.readOnly = true;
    }
}
