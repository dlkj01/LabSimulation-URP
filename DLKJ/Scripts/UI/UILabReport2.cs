using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UILabReport2 : UILabReportBase
{

    [Tooltip("信号源频率")]
    [Header("第一组------------------------------------------------")]
    public InputField inputSourceFrequencyFirst;//信号源频率

    [Tooltip("信号源电压")] public InputField inputSourceVoltageFirst;//信号源电压

    [Tooltip("衰减器设置")] public InputField inputAttenuatorSetupFirst;//衰减器设置

    [Tooltip("驻波比")] public InputField SWRFirst;//驻波比

    [Tooltip("波导波长")] public InputField WaveguideWavelengthFirst;//波导波长

    [Tooltip("第一波节点位置")] public InputField WaveNodePositionFirst;//第一波节点位置

    [Tooltip("归一化负载阻抗")] public InputField NormalizedLoadImpedanceFirst;//归一化负载阻抗

    [Tooltip("负载阻抗")] public InputField LoadImpedanceFirst;//负载阻抗

    [Tooltip("螺钉位置")] public InputField ScrewPositionFirst;//螺钉位置

    [Tooltip("螺钉深度")] public InputField ScrewDepthFirst;//螺钉深度

    [Tooltip("匹配后最小电压")] public InputField MinimumVoltageAfterMatchingFirst;//匹配后最小电压

    [Tooltip("匹配后最大电压")] public InputField MaximumVoltageAfterMatchingFirst;//匹配后最大电压

    [Tooltip("匹配后驻波比")] public InputField SWRAfterMatchingFirst;//匹配后驻波比

    [Tooltip("信号源频率")]
    [Header("第二组------------------------------------------------")]
    public InputField inputSourceFrequencySecond;//信号源频率

    [Tooltip("信号源电压")] public InputField inputSourceVoltageSecond;//信号源电压

    [Tooltip("衰减器设置")] public InputField inputAttenuatorSetupSecond;//衰减器设置

    [Tooltip("驻波比")] public InputField SWRSecond;//驻波比

    [Tooltip("波导波长")] public InputField WaveguideWavelengthSecond;//波导波长

    [Tooltip("第一波节点位置")] public InputField WaveNodePositionSecond;//第一波节点位置

    [Tooltip("归一化负载阻抗")] public InputField NormalizedLoadImpedanceSecond;//归一化负载阻抗

    [Tooltip("负载阻抗")] public InputField LoadImpedanceSecond;//负载阻抗

    [Tooltip("螺钉位置")] public InputField ScrewPositionSecond;//螺钉位置

    [Tooltip("螺钉深度")] public InputField ScrewDepthSecond;//螺钉深度

    [Tooltip("匹配后最小电压")] public InputField MinimumVoltageAfterMatchingSecond;//匹配后最小电压

    [Tooltip("匹配后最大电压")] public InputField MaximumVoltageAfterMatchingSecond;//匹配后最大电压

    [Tooltip("匹配后驻波比")] public InputField SWRAfterMatchingSecond;//匹配后驻波比

}
