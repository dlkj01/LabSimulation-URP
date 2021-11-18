using BestHTTP;
using BestHTTP.WebSocket;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DLKJ
{
    public class SocketManager : MonoBehaviour
    {
        public string address = "ws://www.pixelcattlegames.com:8282";
        private static SocketManager instance;
        public WebSocket webSocket = null;
        public HTTPRequest request = null;


        private Coroutine pingCoroutine;


        public static SocketManager GetInstance()
        {
            if (null == instance)
            {
                instance = (SocketManager)GameObject.FindObjectOfType(typeof(SocketManager));
            }
            return instance;
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            OpenWebSocket();
        }

        void OpenWebSocket()
        {
#if UNITY_WEBGL 
            webSocket = new WebSocket(new Uri(address));
            //request = new HTTPRequest(new Uri(address)).Send();
#endif


#if !BESTHTTP_DISABLE_PROXY
            if (HTTPManager.Proxy != null)
                webSocket.InternalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
#endif

            webSocket.OnOpen += OnOpen;
            webSocket.OnMessage += OnMessageReceived;
            webSocket.OnClosed += OnClosed;
            webSocket.OnError += OnError;

            webSocket.Open();
            Debug.Log("Opening Web Socket...\n");
}

        void OnOpen(WebSocket ws)
        {
            pingCoroutine = StartCoroutine(PingThread());
            Debug.Log("-WebSocket Open!");
        }

        IEnumerator PingThread()
        {
            while (true)
            {
                if (webSocket != null)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic["type"] = "pong";
                    webSocket.Send(BestHTTP.JSON.Json.Encode(dic));
                }
                yield return new WaitForSeconds(5f);
            }
        }

        /// <summary>
        /// Called when we received a text message from the server
        /// </summary>
        void OnMessageReceived(WebSocket ws, string message)
        {

        }

        void OnClosed(WebSocket ws, UInt16 code, string message)
        {
            Debug.Log("-WebSocket closed! Code: {0} Message: " + code + message);
            webSocket = null;
        }

        /// <summary>
        /// Called when an error occured on client side
        /// </summary>
        void OnError(WebSocket ws, string error)
        {
            webSocket = null;
        }

        IEnumerator CheckTimeOut()
        {
            float totalTimes = 10f;
            WaitForSeconds waitForSeconds = new WaitForSeconds(1);
            while (totalTimes > 0)
            {
                yield return waitForSeconds;
                totalTimes--;
            }

           //³¬Ê±ºóµÄÂß¼­
        }

        void OnDestroy()
        {
            if (webSocket != null)
            {
                webSocket.Close();
            }
        }


    }
}