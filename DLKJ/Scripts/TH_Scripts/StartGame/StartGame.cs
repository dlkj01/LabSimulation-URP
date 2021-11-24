using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace DLKJ
{
    public class StartGame : MonoSingleton<StartGame>
    {
        private void Start()
        {
            WordHelper.resultMap.Clear();
        }
        private void OnApplicationQuit()
        {
            if (ProxyManager.saveProxy.IsFinishedAll() == true)
            {
                ProxyManager.saveProxy.Remove();
            }
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F1))
            {
                ProxyManager.saveProxy.Remove();
                SceneManager.didExperiment = false;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
            }

#if UNITY_WEBGL||UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                WordHelper.resultMap.Clear();
                UIManager.GetInstance().UILabButton.uiLabReport.SaveData();
            }
#endif
        }
    }
}