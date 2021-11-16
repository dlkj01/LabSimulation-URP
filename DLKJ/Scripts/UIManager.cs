using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;
using Common;
namespace DLKJ
{
    public class UIManager : MonoBehaviour
    {
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

        [HeaderAttribute("Ƶѡ�ڶ��ӿ�")]
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
            //Debug.unityLogger.logEnabled = false;
            if (sceneObject) sceneObject.SetActive(false);
            // videoShowButton.gameObject.SetActive(false);
            uIVideoPlayer.gameObject.SetActive(false);
            videoShowButton.onClick.AddListener(delegate { ShowVideoButton(); });
            startEquipmentButton.onClick.AddListener(() =>
            {
                string tipString = "��ѹ��˥������Ƶ�����ú󲻿ɸ���";
                switch (SceneManager.GetInstance().currentLab.labName)
                {
                    case SceneManager.THIRD_EXPERIMENT_NAME:
                        tipString = "��ѹ��Ƶ�����ú󲻿ɸ���";
                        break;
                    default:
                        break;
                }

                EventManager.OnTips(TipsType.Snackbar, tipString, () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                    {
                        SetFixDataCallBack();
                        FindObjectOfType<UITips>().OnDisTips();
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
                EventManager.OnTips(TipsType.Toast, "���ķ�����:" + score.ToString("#0.00"), () => { }, () =>
                {
                    Debug.Log("�˳���");
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
            //�Ƿ���Լ���������״̬��ǰ������,Ϊtrue�ſ��Լ���
            if (!VerifyBackLinkIsComplete(currentStep, labName))
                return;
            //��鵱ǰ�����Ƿ���û��д��InputText
            if (ProxyManager.experimentInputProxy.experimentStepInputMap.ContainsKey(currentStep))
            {
                if (!UILabButton.uiLabReport.FinishedStepInput(ProxyManager.experimentInputProxy.experimentStepInputMap[currentStep]))
                {
                    UILabButton.uiLabReport.ShowPanle(false);
                    return;
                }
            }

            //if (labName == SceneManager.SECOND_EXPERIMENT_NAME)
            //{
            //    if (currentStep == 4)
            //    {
            //        EventManager.OnTips(TipsType.Snackbar, "�Ƿ�ʼ�ڶ���ʵ��", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
            //        {
            //            Debug.Log("ˢ���豸,���¸�һ�����ֵ");
            //            NextStep();
            //            if (SceneManager.loginUserData.userType == UserType.Teacher)
            //            {
            //                uiMainPanle.autoConnect.Interactable(true);
            //            }
            //            //��¼��һ�����ݵ�word���
            //            UILabReport2 report2 = UILabButton.uiLabReport as UILabReport2;
            //            report2.WriteInputText();
            //            //���ݳ�ʼ��
            //            MathTool.Init();
            //            //����ڶ���������ȷ��
            //            MathTool.FixedCorrect2SecondGroupCalculate();
            //        });
            //        return;
            //    }
            //}

            if (SceneManager.GetInstance().currentLab.currentStepIndex >= SceneManager.GetInstance().currentLab.steps.Count - 1)
            {
                EventManager.OnTips(TipsType.Toast, "ʵ�����,���ύʵ�鱨��");
                return;
            }

            if (SceneManager.GetInstance().VerifyBasicLink() == false)
            {
                Debug.Log("�������� ����ϸ��飬�۷�");
                SceneManager.GetInstance().currentLab.TriggerScore();
                if (SceneManager.GetInstance().currentLab.currentStep.GetScore() <= 0)
                {
                    //Debug.Log("������û��ֱ�Ӹ�����ȷ��");
                    //verifyButton.interactable = false;
                    //SceneManager.GetInstance().SetBasicItemsLink();
                }
            }
            else
            {
                EventManager.OnTips(TipsType.Snackbar, "ȷ��������һ������", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
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
        /// У��ʵ��������������Ƿ����
        /// </summary>
        public bool VerifyBackLinkIsComplete(int currentStep, string labName)
        {

            if (labName == SceneManager.FIRST_EXPERIMENT_NAME || labName == SceneManager.SECOND_EXPERIMENT_NAME)
            {
                if (currentStep == 2)
                {
                    if (startEquipment == false)
                    {
                        if (SceneManager.GetInstance().VerifyBasicLink() == false)
                        {
                            EventManager.OnTips(TipsType.Toast, "�����豸����");
                        }
                        else
                        {
                            EventManager.OnTips(TipsType.Toast, "�����ʵ�������������");
                        }
                        return false;
                    }
                }
            }


            if (labName == SceneManager.THIRD_EXPERIMENT_NAME)
            {
                if (startEquipment == false)
                {
                    if (currentStep == 1)
                    {
                        if (SceneManager.GetInstance().VerifyBasicLink() == false)
                        {
                            EventManager.OnTips(TipsType.Toast, "�����豸����");
                        }
                        else
                        {
                            EventManager.OnTips(TipsType.Toast, "�����ʵ�������������");
                        }
                        return false;
                    }
                }
            }


            return true;
        }
        private void NextStep()
        {

            Debug.Log("����������ȷ���������һ��ʵ��");
            SceneManager.GetInstance().UpdateItemMoveable(false);
            SceneManager.GetInstance().currentLab.NextStep();
            StepTips(SceneManager.GetInstance().currentLab.currentStep);
            FindObjectOfType<UITips>().OnDisTips();

        }


        /// <summary>
        /// ������ɻ������ݻص�
        /// </summary>
        private void SetFixDataCallBack()
        {
            startEquipment = true;
            startEquipmentButton.gameObject.SetActive(false);
            if (SceneManager.GetInstance().GetInstrumentButton("΢���ź�Դ", "FrequencyBtn2") != null)
            {
                SceneManager.GetInstance().GetInstrumentButton("΢���ź�Դ", "FrequencyBtn2").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("΢���ź�Դ", "FrequencyBtn2").SetInteractiveState(false);
            }
            if (SceneManager.GetInstance().GetInstrumentButton("�ɱ�˥����", "Kebianshaijianqi") != null)
            {
                SceneManager.GetInstance().GetInstrumentButton("�ɱ�˥����", "Kebianshaijianqi").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("�ɱ�˥����", "Kebianshaijianqi").SetInteractiveState(false);
            }
            if (SceneManager.GetInstance().GetInstrumentButton("΢���ź�Դ", "FrequencyBtn") != null)
            {
                SceneManager.GetInstance().GetInstrumentButton("΢���ź�Դ", "FrequencyBtn").RemoveListener();
                SceneManager.GetInstance().GetInstrumentButton("΢���ź�Դ", "FrequencyBtn").SetInteractiveState(false);
            }
            MathTool.Init();
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
                EventManager.OnTips(TipsType.Snackbar, "�Ƿ��˳�����", () => { FindObjectOfType<UITips>().OnDisTips(); },
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
