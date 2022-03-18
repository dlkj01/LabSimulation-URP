using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
using static DLKJ.InstrumentAction;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

namespace DLKJ
{
    public class UIManager : MonoBehaviour
    {
        [Header("Show Datas")]
        public UIShowDatas UIShowDatas;
        public UIShowDatas UIshowDatas1;
        public UIShowDatas UIshowDatas3;
        public bool showUIData = false;

        private static UIManager instance;
        [Header("SceneObject")]
        [SerializeField] GameObject sceneObject;

        [Header("ScrollCamera")]
        [SerializeField] public UI3DCamera _3dCamera;

        [HeaderAttribute("Canvas")]
        public Canvas canvas;

        public Image stepTipsImage;

        [HeaderAttribute("StepTips")]
        public Text stepText;

        [HeaderAttribute("Button")]
        public Button verifyButton;
        [HeaderAttribute("VideoButton")]
        public Button videoShowButton;
        public UIVideoPlayer uIVideoPlayer;

        [HeaderAttribute("频选第二视口")]
        public CanvasGroup voltmeterRect;
        public RawImage voltmeterValue;


        [HeaderAttribute("PanelPrefab")]
        [SerializeField] UIEquipmentPanel equipmentPanelPrefab;
        [SerializeField] UIExperimentSelectedPanel experimentSelectedPanelPrefab;
        [SerializeField] UILabReport1 UILabReport1Prefab;
        [SerializeField] UILabReport2 UILabReport2Prefab;
        [SerializeField] UILabReport3 UILabReport3Prefab;
        public UILabReportController UILabButton;

        public UIMainPanle uiMainPanle;

        private UIEquipmentPanel equipmentPanel = null;
        private UIExperimentSelectedPanel experimentSelectedPanel = null;

        private Camera voltmeterCamera;
        private void Awake()
        {
            Debug.unityLogger.logEnabled = false;
            if (sceneObject) sceneObject.SetActive(false);
            // videoShowButton.gameObject.SetActive(false);
            uIVideoPlayer.gameObject.SetActive(false);
            videoShowButton.onClick.AddListener(delegate { ShowVideoButton(); });
            showUIData = false;
        }
        public void SetStartButton()
        {
            //if (MathTest.Instance.CheckValueIsInit() && startEquipment == false)
            //    startEquipmentButton.gameObject.SetActive(true);

        }

        public static UIManager GetInstance()
        {
            if (null == instance)
            {
                instance = (UIManager)GameObject.FindObjectOfType(typeof(UIManager));
            }
            return instance;
        }

        public void ShowScene()
        {
            if (_3dCamera) _3dCamera.gameObject.SetActive(false);
            sceneObject.SetActive(true);
        }
        public void InitLabReportUI()
        {
            UILabReportBase UILabReport = null;
            switch (SceneManager.GetInstance().currentLab.labName)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport1Prefab);
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport2Prefab);
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport3Prefab);
                    break;
                default:
                    break;
            }
            UILabButton.uiLabReport = UILabReport;
            UILabReport.SetVisibale(false);
        }

        public void ShowEquipmentPanel(int labID)
        {
            if (equipmentPanelPrefab && equipmentPanel == null)
            {
                if (experimentSelectedPanel)
                {
                    Destroy(experimentSelectedPanel.gameObject, 1f);
                }
                equipmentPanel = InstantiateObject(equipmentPanelPrefab);
                equipmentPanel.transform.SetSiblingIndex(experimentSelectedPanel.transform.GetSiblingIndex() - 1);
                equipmentPanel.Initialized(labID);
            }
            else
            {
                Debug.LogWarning("equipmentPanelPrefab is null!");
            }
        }

        public void StepTips(Step step, bool active = true)
        {
            stepTipsImage.gameObject.SetActive(active);
            if (stepText)
            {
                stepText.text = step.stepName;
            }

            if (step.videoClip != null)
            {
                uiMainPanle.videoButton.Interactable(true);
            }
            else
            {
                uiMainPanle.videoButton.Interactable(false);
            }
        }

        public void ShowExperimentSelectedPanel()
        {
            ProxyManager.InitProxy(new SaveProxy("Save"));
            if (experimentSelectedPanelPrefab && experimentSelectedPanel == null)
            {
                experimentSelectedPanel = InstantiateObject(experimentSelectedPanelPrefab);
                experimentSelectedPanel.Initialized();
            }
            else
            {
                Debug.LogWarning("experimentSelectedPanelPrefab is null!");
            }
            if (ProxyManager.saveProxy.IsFinishedAll())
            {
                float score = ProxyManager.saveProxy.GetAllScore();
                EventManager.OnTips(TipsType.Toast, "您的分数是:" + score.ToString("#0.00"), () => { }, () =>
                {
                    Debug.Log("退出了");
                    if (ProxyManager.saveProxy.IsFinishedAll() == true)
                    {
                        ProxyManager.saveProxy.Remove();
                    }
                    Application.Quit();
                });
            }
        }

        public void VerifyBasicLink()
        {
            int currentStep = SceneManager.GetInstance().GetCurrentStep();
            string labName = SceneManager.GetInstance().currentLab.labName;
            //是否可以检查连接完成状态的前提条件,为true才可以继续
            if (!VerifyBackLinkIsComplete(currentStep, labName))
                return;


            //if (labName == SceneManager.SECOND_EXPERIMENT_NAME)
            //{
            //    if (currentStep == 4)
            //    {
            //        EventManager.OnTips(TipsType.Snackbar, "是否开始第二组实验", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
            //        {
            //            Debug.Log("刷新设备,重新给一组随机值");
            //            NextStep();
            //            if (SceneManager.loginUserData.userType == UserType.Teacher)
            //            {
            //                uiMainPanle.autoConnect.Interactable(true);
            //            }
            //            //记录第一组数据到word表格
            //            UILabReport2 report2 = UILabButton.uiLabReport as UILabReport2;
            //            report2.WriteInputText();
            //            //数据初始化
            //            MathTool.Init();
            //            //计算第二组数据正确答案
            //            MathTool.FixedCorrect2SecondGroupCalculate();
            //        });
            //        return;
            //    }
            //}



            if (SceneManager.GetInstance().VerifyBasicLink() == false)
            {
                Debug.Log("连接有误 请仔细检查，扣分");
                EventManager.OnTips(TipsType.Toast, "连接有误,请检查设备连接");
                SceneManager.GetInstance().currentLab.TriggerScore();
                return;
            }
            else
            {
                //检查当前步骤是否有没填写的InputText
                if (ProxyManager.experimentInputProxy.experimentStepInputMap.ContainsKey(currentStep))
                {
                    if (!UILabButton.uiLabReport.FinishedStepInput(ProxyManager.experimentInputProxy.experimentStepInputMap[currentStep]))
                    {
                        UILabButton.uiLabReport.ShowFlashingImage(UILabButton.uiLabReport.GetHeight);
                        return;
                    }
                }
                if (SceneManager.GetInstance().currentLab.currentStepIndex >= SceneManager.GetInstance().currentLab.steps.Count - 1)
                {
                    EventManager.OnTips(TipsType.Toast, "实验完成,请提交实验报告");
                    return;
                }
                EventManager.OnTips(TipsType.Snackbar, "确定进行下一步操作", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                {
                    NextStep();
                    if (SceneManager.loginUserData.userType == UserType.Teacher)
                    {
                        uiMainPanle.autoConnect.Interactable(true);
                    }
                });
            }
        }

        /// <summary>
        /// 校验实验基本数据设置是否完成
        /// </summary>
        public bool VerifyBackLinkIsComplete(int currentStep, string labName)
        {

            if (labName == SceneManager.FIRST_EXPERIMENT_NAME || labName == SceneManager.SECOND_EXPERIMENT_NAME)
            {
                if (currentStep == 3)
                {
                    DataCheck("电压、衰减器、频率设置后不可更改");
                    return false;
                }
            }


            if (labName == SceneManager.THIRD_EXPERIMENT_NAME)
            {
                if (currentStep == 2)
                {
                    DataCheck("电压、频率设置后不可更改");
                    return false;
                }
            }


            return true;
        }
        private void DataCheck(string tipMessage)
        {
            if (MathTest.Instance.CheckValueIsInit() == false)
            {
                EventManager.OnTips(TipsType.Toast, "请完成实验基本数据设置");
                return;
            }
            InstrumentButton buttonKaiGuan = SceneManager.GetInstance().GetInstrumentButton("选频放大器", "FrequencySelectiveAmplifierPowerBtn");
            if (MathUtility.GetCurrentValue(buttonKaiGuan) != 0)
            {
                EventManager.OnTips(TipsType.Toast, "请打开选频放大器开关");
                return;
            }
            EventManager.OnTips(TipsType.Snackbar, tipMessage, () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
            {
                FindObjectOfType<UITips>().OnDisTips();
                MathTool.Init();
                SetFixDataCallBack();
                showUIData = true;
            });
            return;
        }
        private void NextStep()
        {

            Debug.Log("基础连接正确，请进行下一步实验");
            SceneManager.GetInstance().UpdateItemMoveable(false);
            SceneManager.GetInstance().currentLab.NextStep();
            StepTips(SceneManager.GetInstance().currentLab.currentStep);
            FindObjectOfType<UITips>().OnDisTips();

        }


        /// <summary>
        /// 设置完成基本数据回调
        /// </summary>
        private void SetFixDataCallBack()
        {
            //关闭这些设备可交互
            if (SceneManager.GetInstance().GetInstrumentButton("微波信号源", "FrequencyBtn2") != null)
            {
                SceneManager.GetInstance().GetInstrumentButton("微波信号源", "FrequencyBtn2").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("微波信号源", "FrequencyBtn2").SetInteractiveState(false);
            }
            if (SceneManager.GetInstance().GetInstrumentButton("可变衰减器", "Kebianshaijianqi") != null)
            {
                SceneManager.GetInstance().GetInstrumentButton("可变衰减器", "Kebianshaijianqi").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("可变衰减器", "Kebianshaijianqi").SetInteractiveState(false);
            }
            if (SceneManager.GetInstance().GetInstrumentButton("微波信号源", "FrequencyBtn") != null)
            {
                SceneManager.GetInstance().GetInstrumentButton("微波信号源", "FrequencyBtn").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("微波信号源", "FrequencyBtn").SetInteractiveState(false);
            }
            //计算正确答案
            //MathTool.Init();
            switch (SceneManager.GetInstance().currentLab.labName)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    MathTool.FixedCorrect1Calculate();
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    MathTool.FixedCorrect2FirstGroupCalculate();
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    MathTool.FixedCorrect3Calculate();
                    break;
                default:
                    break;
            }
            //写入固定的Input用户不可更改
            UILabButton.uiLabReport.SetInputTextReadOnly();
            NextStep();
            if (SceneManager.loginUserData.userType == UserType.Teacher)
            {
                uiMainPanle.autoConnect.Interactable(true);
            }
        }

        public void SetVerifyButtonActive(bool active) { verifyButton.gameObject.SetActive(active); }


        public T InstantiateObject<T>(T prefab) where T : Component
        {
            var newObject = Instantiate<T>(prefab, canvas.transform);
            return newObject;
        }

        public void SetVoltmeterValue(Camera camera)
        {
            voltmeterCamera = camera;
            voltmeterValue.texture = camera.targetTexture;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventManager.OnTips(TipsType.Snackbar, "是否退出程序", () => { FindObjectOfType<UITips>().OnDisTips(); },
                () =>
                {
                    Application.Quit();
                });
            }
        }

        public void ShowVideoButton()
        {
            if (uIVideoPlayer.gameObject.activeSelf)
            {
                uIVideoPlayer.gameObject.SetActive(false);
                uIVideoPlayer.Reset();
            }
            else
            {
                uIVideoPlayer.Play();
                uIVideoPlayer.gameObject.SetActive(true);
            }
        }
        public void PinXuanView()
        {
            voltmeterRect.DOKill();
            voltmeterRect.DOFade(1, 0.3f).OnComplete(() =>
             {
                 voltmeterRect.DOFade(1, 4f).OnComplete(() => { voltmeterRect.DOFade(0, 2f); });

             });
        }

        public void DownloadFile(string path, string fileName)
        {
            // 如 path = Application.streamingAssetsPath;
            // fileName = "LabReport3.docx"；
#if UNITY_WEBGL
            StartCoroutine(DownLoadDoc(path, fileName));
#endif
        }


        /// <summary>
        /// 实验报告下载
        /// </summary>
        /// <param name="path">截止到文件名称的文件所在路径</param> 如 Application.streamingAssetsPath
        /// <param name="fileName">带文件格式的完整文件名称</param> 如 LabReport3.docx
        /// <returns></returns>
        IEnumerator DownLoadDoc(string path, string fileName)
        {
            System.Uri uri = new System.Uri(Path.Combine(path, fileName));
            UnityWebRequest request = UnityWebRequest.Get(uri);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                WebGLDownloadHelper.DownloadDocx(request.downloadHandler.data, fileName);
            }
        }

    }



}
