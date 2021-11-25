using BestHTTP;
using BestHTTP.WebSocket;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
            //OpenWebSocket();
        }

        void OpenWebSocket()
        {
#if UNITY_WEBGL 
            webSocket = new WebSocket(new Uri(address));
            //request = new HTTPRequest(new Uri(address)).Send();
#endif


#if !BESTHTTP_DISABLE_PROXY
            //if (HTTPManager.Proxy != null)
            //    webSocket.OnInternalRequestCreated = (ws, internalRequest) => internalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
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

            //超时后的逻辑
        }

        void OnDestroy()
        {
            if (webSocket != null)
            {
                webSocket.Close();
            }
        }

        public void Test()
        {
            WordHelper.resultMap.Clear();
            UIManager.GetInstance().UILabButton.uiLabReport.SaveData();


            Dictionary<string, List<string>> stepsDic = new Dictionary<string, List<string>>();

            List<string> stepsJson = new List<string>();
            for (int i = 0; i < 3; i++)
            {
                Dictionary<string, string> dic = ReturnStep();
                string jsonKey = BestHTTP.JSON.Json.Encode(dic);
                stepsJson.Add(jsonKey);
            }
            stepsDic.Add("steps", stepsJson);

            string stepStr = BestHTTP.JSON.Json.Encode(stepsDic);
            Debug.Log("实验步骤:" + stepStr);

            //发送实验步骤
            SendReportToWeb(stepStr);

            //发送得分
            SendScoreToWeb(99);

            //发送报告
            Dictionary<string, List<Dictionary<string, string>>> dic1 = Report1();
            string jsonKey1 = BestHTTP.JSON.Json.Encode(dic1);
          
            Debug.Log("报告String:"+ jsonKey1);
            SendReportToWeb(jsonKey1);

            //获取用户信息
            //getUserInfo();
        }

        int i = 0;
        Dictionary<string, string> ReturnStep()
        {
            i++;
            Dictionary<string, string> dic = new Dictionary<string, string>();

            dic["seq"] = i.ToString();
            dic["title"] = "虚拟仿真实验";
            dic["startTime"] = "2021-11-19 09:30:48";
            dic["endTime"] = "2021-11-19 09:35:48";
            dic["expectTime"] = "1000";
            dic["maxScore"] = "100";
            dic["score"] = "60";
            dic["repeatCount"] = "3";
            dic["evaluation"] = "暂无";
            dic["scoringModel"] = "暂无";
            dic["remarks"] = "暂无";
            return dic;
        }

        /// <summary>
        /// webgl端成绩上传
        /// </summary>
        /// <param name="score">实验成绩</param>
        public void SendScoreToWeb(int score)
        {
            string[] moduleFlag = { "实验成绩" };
            string[] questionNumber = { "1" };
            string[] questionStem = { "学生实验操作成绩" };
            string[] scores = { score.ToString() };
            string[] isTrue = { "True" };
            Application.ExternalCall("ReciveData", moduleFlag, questionNumber, questionStem, scores, isTrue);
        }

        /// <summary>
        /// webgl提交实验报告
        /// </summary>
        /// <param name="jsonReslut">json格式报告字符串</param>
        public void SendReportToWeb(string jsonReslut)
        {
            Application.ExternalCall("ReportEdit", jsonReslut);
        }

        Dictionary<string, List<Dictionary<string, string>>> Report1()
        {
            Dictionary<string, List<Dictionary<string, string>>> dic = new Dictionary<string, List<Dictionary<string, string>>>();
            Dictionary<string, string> keyValuePairs = WordHelper.resultMap;

            string identifier = "text";
            string key;
            string value;
            string[] keysArry = { "text", "color" };
            string[] valueArry = new string[2];

            int index = 0;
            foreach (var item in keyValuePairs)
            {
                index++;
                // key = item.Key.ToString();
                key = identifier + index.ToString();
                value = item.Value.ToString();
                List<Dictionary<string,string>> contentValueList = new List<Dictionary<string,string>>();
                Dictionary<string, string> contentDic = new Dictionary<string, string>();
                for (int i = 0; i < 2; i++)
                {
                    switch (i)
                    {
                        case 0:
                            {
                                valueArry[0] = value;
                            }
                            break;
                        case 1:
                            {
                                valueArry[1] = "red";
                            }
                            break;
                        default:
                            break;
                    }
                    contentDic.Add(keysArry[i], valueArry[i]);
                }
                contentValueList.Add(contentDic);
                dic.Add(key, contentValueList);
            }
            return dic;
        }

    }
}