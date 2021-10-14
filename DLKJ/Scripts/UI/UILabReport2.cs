using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILabReport2 : UILabReportBase
{

    [Tooltip("�ź�ԴƵ��")]
    [Header("��һ��------------------------------------------------")]
    public InputField inputSourceFrequencyFirst;//�ź�ԴƵ��

    [Tooltip("�ź�Դ��ѹ")] public InputField inputSourceVoltageFirst;//�ź�Դ��ѹ

    [Tooltip("˥��������")] public InputField inputAttenuatorSetupFirst;//˥��������

    [Tooltip("פ����")] public InputField SWRFirst;//פ����

    [Tooltip("��������")] public InputField WaveguideWavelengthFirst;//��������

    [Tooltip("��һ���ڵ�λ��")] public InputField WaveNodePositionFirst;//��һ���ڵ�λ��

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceFirst;//��һ�������迹

    [Tooltip("�����迹")] public InputField LoadImpedanceFirst;//�����迹

    [Tooltip("�ݶ�λ��")] public InputField ScrewPositionFirst;//�ݶ�λ��

    [Tooltip("�ݶ����")] public InputField ScrewDepthFirst;//�ݶ����

    [Tooltip("ƥ�����С��ѹ")] public InputField MinimumVoltageAfterMatchingFirst;//ƥ�����С��ѹ

    [Tooltip("ƥ�������ѹ")] public InputField MaximumVoltageAfterMatchingFirst;//ƥ�������ѹ

    [Tooltip("ƥ���פ����")] public InputField SWRAfterMatchingFirst;//ƥ���פ����

    [Tooltip("�ź�ԴƵ��")]
    [Header("�ڶ���------------------------------------------------")]
    public InputField inputSourceFrequencySecond;//�ź�ԴƵ��

    [Tooltip("�ź�Դ��ѹ")] public InputField inputSourceVoltageSecond;//�ź�Դ��ѹ

    [Tooltip("˥��������")] public InputField inputAttenuatorSetupSecond;//˥��������

    [Tooltip("פ����")] public InputField SWRSecond;//פ����

    [Tooltip("��������")] public InputField WaveguideWavelengthSecond;//��������

    [Tooltip("��һ���ڵ�λ��")] public InputField WaveNodePositionSecond;//��һ���ڵ�λ��

    [Tooltip("��һ�������迹")] public InputField NormalizedLoadImpedanceSecond;//��һ�������迹

    [Tooltip("�����迹")] public InputField LoadImpedanceSecond;//�����迹

    [Tooltip("�ݶ�λ��")] public InputField ScrewPositionSecond;//�ݶ�λ��

    [Tooltip("�ݶ����")] public InputField ScrewDepthSecond;//�ݶ����

    [Tooltip("ƥ�����С��ѹ")] public InputField MinimumVoltageAfterMatchingSecond;//ƥ�����С��ѹ

    [Tooltip("ƥ�������ѹ")] public InputField MaximumVoltageAfterMatchingSecond;//ƥ�������ѹ

    [Tooltip("ƥ���פ����")] public InputField SWRAfterMatchingSecond;//ƥ���פ����

}
