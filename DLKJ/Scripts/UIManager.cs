using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace DLKJ
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [Header("SceneObject")]
        [SerializeField] GameObject sceneObject;

        [Header("ScrollCamera")]
        [SerializeField]public UI3DCamera _3dCamera;

        [HeaderAttribute("Canvas")]
        public Canvas canvas;

        [HeaderAttribute("StepTips")]
        public Text stepText;

        [HeaderAttribute("Button")]
        public Button verifyButton;
        [HeaderAttribute("VideoButton")]
        public Button videoShowButton;
        public UIVideoPlayer uIVideoPlayer;
       
        [HeaderAttribute("Ƶѡ�ڶ��ӿ�")]
        public RectTransform voltmeterRect;
        public RawImage voltmeterValue;
        public Button voltmeterButton;


        [HeaderAttribute("PanelPrefab")]
        [SerializeField] UIEquipmentPanel equipmentPanelPrefab;
        [SerializeField] UIExperimentSelectedPanel experimentSelectedPanelPrefab;
        [SerializeField] UILabReport1 UILabReport1Prefab;
        [SerializeField] UILabReport2 UILabReport2Prefab;
        [SerializeField] UILabReport3 UILabReport3Prefab;
        public UILabReportController UILabButton;

        private UIEquipmentPanel equipmentPanel = null;
        private UIExperimentSelectedPanel experimentSelectedPanel = null;

        private Camera voltmeterCamera;

        private void Awake()
        {
            if (sceneObject) sceneObject.SetActive(false);
           // videoShowButton.gameObject.SetActive(false);
            uIVideoPlayer.gameObject.SetActive(false);
            videoShowButton.onClick.AddListener(delegate { ShowVideoButton(); });
            voltmeterButton.onClick.AddListener(delegate { PinXuanView(); });
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
        public void CreatLabReportUI()
        {
            UILabReportBase UILabReport = null;
            switch (SceneManager.GetInstance().currentLab.labName)
            {
                case "���˿�΢�������������":
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport1Prefab);
                    break;
                case "�����迹����":
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport2Prefab);
                    break;
                case " �����迹ƥ��Ͷ�����������ԵĲ���":
                    UILabReport = InstantiateObject<UILabReportBase>(UILabReport3Prefab);
                    break;
                default:
                    break;
            }
            UILabButton.uiLabReport = UILabReport;
            UILabButton.gameObject.SetActive(true);
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

        public void StepTips(Step step)
        {
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
        }

        public void VerifyBasicLink()
        {
            if (SceneManager.GetInstance().VerifyBasicLink())
            {
                Debug.Log("����������ȷ���������һ��ʵ��");
                SceneManager.GetInstance().UpdateItemMoveable(false);
                SceneManager.GetInstance().currentLab.NextStep();
                StepTips(SceneManager.GetInstance().currentLab.currentStep);
            }
            else
            {
                Debug.Log("�������� ����ϸ��飬�۷�");
                SceneManager.GetInstance().currentLab.TriggerScore();


                if (SceneManager.GetInstance().currentLab.currentStep.GetScore() <= 0)
                {
                    Debug.Log("������û��ֱ�Ӹ�����ȷ��");
                    verifyButton.interactable = false;
                    SceneManager.GetInstance().SetBasicItemsLink();
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
                EventManager.OnTips(TipsType.Snackbar, "�Ƿ��˳�����", () => { FindObjectOfType<UITips>().OnDisTips(); },
                () =>
                {
                    Application.Quit();
                });
            }
        }

        void ShowVideoButton()
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
        void PinXuanView()
        {
            if (voltmeterRect.rect.width > 0)
            {
                if (systolic != null) StopCoroutine(systolic);
                systolic =  StartCoroutine(Systolic());
            }
            else
            {
                if (flexible != null) StopCoroutine(flexible);
                flexible = StartCoroutine(Flexible());
            }
        }

        IEnumerator Systolic()
        {
            float value = voltmeterRect.rect.width;
            while (value > 0)
            {
                value--;
                voltmeterRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                yield return null;
            }
        }

        IEnumerator Flexible()
        {
            float value = 0;
            while (value <= 400)
            {
                value++;
                voltmeterRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
                yield return null;
            }
        }

    }



}
