using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;

        [Header("SceneObject")]
        [SerializeField] GameObject sceneObject;

        [Header("ScrollCamera")]
        [SerializeField] UI3DCamera _3dCamera;

        [HeaderAttribute("Canvas")]
        public Canvas canvas;

        [HeaderAttribute("StepTips")]
        public Text stepText;

        [HeaderAttribute("Button")]
        public Button verifyButton;

        [HeaderAttribute("PanelPrefab")]
        [SerializeField] UIEquipmentPanel equipmentPanelPrefab;
        [SerializeField] UIExperimentSelectedPanel experimentSelectedPanelPrefab;



        private UIEquipmentPanel equipmentPanel = null;
        private UIExperimentSelectedPanel experimentSelectedPanel = null;

        private void Awake()
        {
            if (sceneObject) sceneObject.SetActive(false);
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
            if (experimentSelectedPanelPrefab&& experimentSelectedPanel == null)
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
                Debug.Log("基础连接正确，请进行下一步实验");
                SceneManager.GetInstance().UpdateItemMoveable(false);
                SceneManager.GetInstance().currentLab.NextStep();
                StepTips(SceneManager.GetInstance().currentLab.currentStep);
            }
            else
            {
                Debug.Log("连接有误 请仔细检查，扣分");
                SceneManager.GetInstance().currentLab.TriggerScore();


                if (SceneManager.GetInstance().currentLab.currentStep.GetScore()<=0)
                {
                    Debug.Log("分数扣没，直接给出正确答案");
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

        
    }

   

}
