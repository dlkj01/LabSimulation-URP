using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    public class ProxyManager : Singleton<ProxyManager>
    {
        private readonly ConcurrentDictionary<string, BaseProxy> m_ProxyMap = new ConcurrentDictionary<string, BaseProxy>();

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="proxy"></param>
        public void RegisterProxy(BaseProxy proxy)
        {
            m_ProxyMap[proxy.ProxyName] = proxy;
            proxy.Register();
        }
        /// <summary>
        /// 拿
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyName"></param>
        /// <returns></returns>
        public T RetrieveProxy<T>(string proxyName) where T : BaseProxy
        {
            return (T)(m_ProxyMap.TryGetValue(proxyName, out var proxy) ? proxy : null);
        }

        public BaseProxy RetrieveProxy(string proxyName)
        {
            return m_ProxyMap.TryGetValue(proxyName, out var proxy) ? proxy : null;
        }

        public BaseProxy RemoveProxy(string proxyName)
        {
            if (m_ProxyMap.TryRemove(proxyName, out var proxy))
            {
                proxy.Remove();
            }
            return proxy;
        }

        /// <summary>
        /// 初始化一个Proxy
        /// </summary>
        public static void InitProxy(BaseProxy proxy)
        {
            GetInstance.RegisterProxy(proxy);
        }
        public static SaveProxy saveProxy { get { return GetInstance.RetrieveProxy<SaveProxy>("Save"); } }
        public static ExperimentInputVerifyProxy experimentInputProxy { get { return GetInstance.RetrieveProxy<ExperimentInputVerifyProxy>("ExperimentInputVerify"); } }
    }
}