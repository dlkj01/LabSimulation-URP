using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace DLKJ
{
    public class StartGame : MonoSingleton<StartGame>
    {
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
            }
        }
    }
}