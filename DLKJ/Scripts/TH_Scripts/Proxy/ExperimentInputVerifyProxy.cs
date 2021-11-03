using DLKJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentInputVerifyProxy : BaseProxy
{
    private ExperimentInputVerifyExcelData experimentInputVerifyExcelData { get; set; }
    public Dictionary<int, string[]> experimentStepInputMap = new Dictionary<int, string[]>();//����====>inputtext
    public ExperimentInputVerifyProxy(string proxyName, object data = null) : base(proxyName, data) { }
    public override void Register()
    {
        experimentInputVerifyExcelData = ExcelManager.GetInstance.GetExcelData<ExperimentInputVerifyExcelData, ExperimentInputVerifyExcelItem>();
        string labName = SceneManager.GetInstance().currentLab.labName;
        foreach (var item in experimentInputVerifyExcelData.items)
        {
            switch (labName)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    if (item.ExperimentalType == "ʵ��һ")
                        experimentStepInputMap.Add(item.ExperimentalStep, item.InputTextName);
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    if (item.ExperimentalType == "ʵ���")
                        experimentStepInputMap.Add(item.ExperimentalStep, item.InputTextName);
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    if (item.ExperimentalType == "ʵ����")
                        experimentStepInputMap.Add(item.ExperimentalStep, item.InputTextName);
                    break;
                default:
                    break;
            }
        }
    }
}
