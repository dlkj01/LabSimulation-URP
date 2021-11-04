using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using DLKJ;
public struct SaveData
{
    public string experimentName;
    public float score;
    public bool isFinished;
}
public struct SaveDataList
{
    public SaveData Experiment1SaveData;
    public SaveData Experiment2SaveData;
    public SaveData Experiment3SaveData;
}

public class SaveProxy : BaseProxy
{
    public SaveDataList allExperimentSaveData;
    public Dictionary<string, SaveData> map = new Dictionary<string, SaveData>();
    public SaveProxy(string proxyName, object data = null) : base(proxyName, data) { }
    public override void Register()
    {
        allExperimentSaveData = ES3.Load<SaveDataList>(SceneManager.loginUserData.accountNumber, allExperimentSaveData);
        map.Add(SceneManager.FIRST_EXPERIMENT_NAME, allExperimentSaveData.Experiment1SaveData);
        map.Add(SceneManager.SECOND_EXPERIMENT_NAME, allExperimentSaveData.Experiment2SaveData);
        map.Add(SceneManager.THIRD_EXPERIMENT_NAME, allExperimentSaveData.Experiment3SaveData);
    }
    public float GetAllScore()
    {
        return allExperimentSaveData.Experiment1SaveData.score + allExperimentSaveData.Experiment2SaveData.score + allExperimentSaveData.Experiment3SaveData.score;
    }

    public bool IsFinishedAll()
    {
        return allExperimentSaveData.Experiment1SaveData.isFinished && allExperimentSaveData.Experiment2SaveData.isFinished && allExperimentSaveData.Experiment3SaveData.isFinished;
    }
    public override void Save()
    {
        base.Save();
        switch (SceneManager.GetInstance().GetCurrentLabName())
        {
            case SceneManager.FIRST_EXPERIMENT_NAME:
                allExperimentSaveData.Experiment1SaveData.experimentName = SceneManager.FIRST_EXPERIMENT_NAME;
                allExperimentSaveData.Experiment1SaveData.isFinished = true;
                allExperimentSaveData.Experiment1SaveData.score = SceneManager.GetInstance().currentLabScore;
                break;
            case SceneManager.SECOND_EXPERIMENT_NAME:
                allExperimentSaveData.Experiment2SaveData.experimentName = SceneManager.SECOND_EXPERIMENT_NAME;
                allExperimentSaveData.Experiment2SaveData.isFinished = true;
                allExperimentSaveData.Experiment2SaveData.score = SceneManager.GetInstance().currentLabScore;
                break;
            case SceneManager.THIRD_EXPERIMENT_NAME:
                allExperimentSaveData.Experiment3SaveData.experimentName = SceneManager.THIRD_EXPERIMENT_NAME;
                allExperimentSaveData.Experiment3SaveData.isFinished = true;
                allExperimentSaveData.Experiment3SaveData.score = SceneManager.GetInstance().currentLabScore;
                break;
            default:
                break;
        }
        ES3.Save(SceneManager.loginUserData.accountNumber, allExperimentSaveData);
    }
    public override void Remove()
    {
        base.Remove();
        ES3.DeleteKey(SceneManager.loginUserData.accountNumber);
    }
}
