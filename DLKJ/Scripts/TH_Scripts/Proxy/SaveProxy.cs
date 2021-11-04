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

public class SaveProxy : BaseProxy
{
    private string completedStr = "Finished";
    public SaveData Experiment1SaveData;
    public SaveData Experiment2SaveData;
    public SaveData Experiment3SaveData;
    public Dictionary<string, SaveData> map = new Dictionary<string, SaveData>();
    public SaveProxy(string proxyName, object data = null) : base(proxyName, data) { }
    private string firstResult, secondResult, thirdResult;
    public override void Register()
    {
        firstResult = PlayerPrefs.GetString(SceneManager.FIRST_EXPERIMENT_NAME);
        secondResult = PlayerPrefs.GetString(SceneManager.SECOND_EXPERIMENT_NAME);
        thirdResult = PlayerPrefs.GetString(SceneManager.THIRD_EXPERIMENT_NAME);
        Experiment1SaveData.experimentName = SceneManager.FIRST_EXPERIMENT_NAME;
        Experiment2SaveData.experimentName = SceneManager.SECOND_EXPERIMENT_NAME;
        Experiment3SaveData.experimentName = SceneManager.THIRD_EXPERIMENT_NAME;
        if (firstResult == completedStr)
        {
            Experiment1SaveData.isFinished = true;
            Experiment1SaveData.score = PlayerPrefs.GetFloat(SceneManager.FIRST_EXPERIMENT_NAME + "Score");
        }
        if (secondResult == completedStr)
        {
            Experiment2SaveData.isFinished = true;
            Experiment2SaveData.score = PlayerPrefs.GetFloat(SceneManager.SECOND_EXPERIMENT_NAME + "Score");
        }
        if (thirdResult == completedStr)
        {
            Experiment3SaveData.isFinished = true;
            Experiment3SaveData.score = PlayerPrefs.GetFloat(SceneManager.THIRD_EXPERIMENT_NAME + "Score");
        }
        map.Add(SceneManager.FIRST_EXPERIMENT_NAME, Experiment1SaveData);
        map.Add(SceneManager.SECOND_EXPERIMENT_NAME, Experiment2SaveData);
        map.Add(SceneManager.THIRD_EXPERIMENT_NAME, Experiment3SaveData);

    }
    /// <summary>
    /// 提交实验报告时调用一次
    /// </summary>
    /// <param name="key"></param>
    /// <param name="score"></param>
    public void SetData(string key, float score)
    {
        PlayerPrefs.SetString(key, completedStr);
        PlayerPrefs.SetFloat(key + "Score", score);
    }
    public float GetAllScore()
    {
        return Experiment1SaveData.score + Experiment2SaveData.score + Experiment3SaveData.score;
    }

    public bool IsFinishedAll()
    {
        return firstResult == completedStr && secondResult == completedStr && thirdResult == completedStr;
    }
    public bool VerifyIsFinished(string experimentName)
    {
        return map[experimentName].isFinished;
    }
    public override void Save()
    {
        base.Save();
        PlayerPrefs.Save();
    }
    public override void Remove()
    {
        base.Remove();
        PlayerPrefs.DeleteAll();
    }
}
