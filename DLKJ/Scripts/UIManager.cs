using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

namespace DLKJ
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static List<string> experimentID = new List<string>();
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

        public Button startEquipmentButton;
        bool startEquipment = false;

        private void Awake()
        {
            if (sceneObject) sceneObject.SetActive(false);
            // videoShowButton.gameObject.SetActive(false);
            uIVideoPlayer.gameObject.SetActive(false);
            videoShowButton.onClick.AddListener(delegate { ShowVideoButton(); });
            startEquipmentButton.onClick.AddListener(() =>
            {
                EventManager.OnTips(TipsType.Snackbar, "电压、衰减器、频率设置后不可更改", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                    {
                        startEquipment = true;
                        startEquipmentButton.gameObject.SetActive(false);
                        if (SceneManager.GetInstance().GetInstrumentButton("频选放大器", "RotaryBtnVoltage") != null)
                        {
                            SceneManager.GetInstance().GetInstrumentButton("频选放大器", "RotaryBtnVoltage").RemoveListener();
                            SceneManager.GetInstance().GetInstrumentButton("频选放大器", "RotaryBtnVoltage").SetInteractiveState(false);
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
                        FindObjectOfType<UITips>().OnDisTips();
                        MathTool.Init();
                        switch (SceneManager.GetInstance().currentLab.labName)
                        {
                            case "二端口微波网络参量测量":
                                MathTool.FixedCorrect1Calculate();
                                break;
                            case "负载阻抗测量":
                                MathTool.FixedCorrect2FirstGroupCalculate();
                                break;
                            case "负载阻抗匹配和定向耦合器特性的测量":
                                MathTool.FixedCorrectCalculate();
                                break;
                            default:
                                break;
                        }

                    });
            });
        }
        public void SetStartButton()
        {
            if (MathTest.Instance.CheckValueIsInit() && startEquipment == false)
                startEquipmentButton.gameObject.SetActive(true);

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
                case "二端口微波网络参量测量":
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport1Prefab);
                    break;
                case "负载阻抗测量":
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport2Prefab);
                    break;
                case "负载阻抗匹配和定向耦合器特性的测量":
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
                if (experimentSelectedPanel) DestroyImmediate(experimentSelectedPanel.gameObject);

                equipmentPanel = InstantiateObject(equipmentPanelPrefab);
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
        }

        public void ShowExperimentSelectedPanel()
        {
            if (experimentSelectedPanelPrefab && experimentSelectedPanel == null)
            {
                experimentSelectedPanel = InstantiateObject(experimentSelectedPanelPrefab);
                experimentSelectedPanel.Initialized();
            }
            else
            {
                Debug.LogWarning("experimentSelectedPanelPrefab is null!");
            }
            if (experimentID.Count >= 3)
            {
                EventManager.OnTips(TipsType.Toast, "所有实验已经完成,您的分数是:" + MathTool.score.ToString(), () => { }, () =>
                 {
                     Debug.Log("退出了");
                     Application.Quit();
                 });
            }
        }

        public void VerifyBasicLink()
        {
            string labName = SceneManager.GetInstance().currentLab.labName;
            int currentStep = SceneManager.GetInstance().currentLab.currentStepIndex;
            if (labName == "二端口微波网络参量测量" || labName == "负载阻抗测量")
            {
                if (startEquipment == false)
                {
                    if (currentStep == 2)
                    {
                        EventManager.OnTips(TipsType.Toast, "请完成实验基本数据设置");
                        return;
                    }
                }
            }

            if (labName == "负载阻抗测量")
            {
                if (currentStep == 4)
                {
                    EventManager.OnTips(TipsType.Snackbar, "是否开始第二组实验", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                       {
                           Debug.Log("刷新设备,重新给一组随机值");
                           //下一步
                           SceneManager.GetInstance().UpdateItemMoveable(false);
                           SceneManager.GetInstance().currentLab.NextStep();
                           StepTips(SceneManager.GetInstance().currentLab.currentStep);
                           FindObjectOfType<UITips>().OnDisTips();
                           //记录第一组数据到word表格
                           UILabReport2 report2 = UILabButton.uiLabReport as UILabReport2;
                           report2.CacheFirstGroupData();
                           report2.WriteInputText(WordHelper.cacheData, WordHelper.cacheUserData);
                           //数据初始化
                           MathTool.Init();
                           //计算第二组数据正确答案
                           MathTool.FixedCorrect2SecondGroupCalculate();
                       });
                    return;
                }
            }

            if (labName == "负载阻抗匹配和定向耦合器特性的测量")
            {
                if (startEquipment == false)
                {
                    if (currentStep == 1)
                    {
                        EventManager.OnTips(TipsType.Toast, "请完成实验基本数据设置");
                        return;
                    }
                }
            }

            if (SceneManager.GetInstance().currentLab.currentStepIndex >= SceneManager.GetInstance().currentLab.steps.Count - 1)
            {
                EventManager.OnTips(TipsType.Toast, "实验完成,请提交实验报告");
            }

            if (SceneManager.GetInstance().VerifyBasicLink())
            {
                Debug.Log("基础连接正确，请进行下一步实验");
                SceneManager.GetInstance().UpdateItemMoveable(false);
                SceneManager.GetInstance().currentLab.NextStep();
                StepTips(SceneManager.GetInstance().currentLab.currentStep);
            }
            else
            {
                Debug.Log("连接有误 请仔细检查，扣分");
                SceneManager.GetInstance().currentLab.TriggerScore();


                if (SceneManager.GetInstance().currentLab.currentStep.GetScore() <= 0)
                {
                    //Debug.Log("分数扣没，直接给出正确答案");
                    //verifyButton.interactable = false;
                    //SceneManager.GetInstance().SetBasicItemsLink();
                }
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

        private Coroutine systolic = null;
        private Coroutine flexible = null;
        public void PinXuanView()
        {
            voltmeterRect.DOKill();
            voltmeterRect.DOFade(1, 0.3f).OnComplete(() =>
             {
                 voltmeterRect.DOFade(1, 4f).OnComplete(() => { voltmeterRect.DOFade(0, 2f); });

             });
        }

    }



}
