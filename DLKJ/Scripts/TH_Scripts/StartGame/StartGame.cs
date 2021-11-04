using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace DLKJ
{
    public class StartGame : MonoSingleton<StartGame>
    {
        private void Awake()
        {
            ProxyManager.InitProxy(new SaveProxy("Save"));
        }
    }
}