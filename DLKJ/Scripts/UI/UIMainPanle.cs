using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Common;
namespace DLKJ
{
    public class UIMainPanle : MonoBehaviour
    {
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float normalX;
        [SerializeField] private float targetX;
        [SerializeField] private Transform arrow;
        [SerializeField] private Image controllerButton;
        private bool isMoving;//正在移动？
        private bool isHide;//是否隐藏了
        RectTransform rectTF;
        public UIButton autoConnect;
        public UIButton videoButton;

        private void Awake()
        {
            rectTF = transform as RectTransform;
            UIEventListener.GetUIEventListener(controllerButton.gameObject).PointerClick += (p) =>
              {
                  MovePanle();
              };
        }
        public void Init(UserType type)
        {
            switch (type)
            {
                case UserType.Null:
                    break;
                case UserType.Student:
                    break;
                case UserType.Teacher:
                    autoConnect.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        public void MovePanle()
        {
            if (isMoving == true) return;
            transform.DOKill();
            isMoving = true;
            if (isHide)
            {
                arrow.transform.localEulerAngles = new Vector3(0, 0, 180);
                rectTF.DOAnchorPos3DX(targetX, duration).OnComplete(() =>
                {
                    isMoving = false;
                    isHide = false;
                });
            }
            else
            {
                arrow.transform.localEulerAngles = Vector3.zero;
                rectTF.DOAnchorPos3DX(normalX, duration).OnComplete(() =>
                {
                    isMoving = false;
                    isHide = true;
                });
            }
        }

        public void SetPanleActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void UseDevice()
        {
            List<Item> allItems = SceneManager.GetInstance().GetUsingItems();

            foreach (var item in allItems)
            {
                BoxCollider boxCollider;

                if (item.TryGetComponent<BoxCollider>(out boxCollider))
                {
                    boxCollider.enabled = false;
                }
                MeshCollider meshCollider;
                if (item.TryGetComponent<MeshCollider>(out meshCollider))
                {
                    meshCollider.enabled = false;
                }
            }
            SceneManager.GetInstance().SetMouseState(false, true);
        }

        public void MoveDevice()
        {
            List<Item> allItems = SceneManager.GetInstance().GetUsingItems();
            foreach (var item in allItems)
            {
                BoxCollider boxCollider;
                if (item.TryGetComponent<BoxCollider>(out boxCollider))
                {
                    boxCollider.enabled = true;
                }
                MeshCollider meshCollider;
                if (item.TryGetComponent<MeshCollider>(out meshCollider))
                {
                    meshCollider.enabled = true;
                }
            }
            SceneManager.GetInstance().SetMouseState(false, false);
        }

        public void ViewTheVideo()
        {
            if (SceneManager.GetInstance().currentLab.currentStepIndex >= 2)
            {
                UIManager.GetInstance().ShowVideoButton();
            }
            else
            {
                EventManager.OnTips(TipsType.Toast, "请完成基础连接");
            }
        }

        public void ViewLabReport()
        {
            UIManager.GetInstance().UILabButton.uiLabReport.SetVisibale(true);
        }

        public void SubmitTest()
        {
#if !UNITY_EDITOR
            if (SceneManager.GetInstance().GetCurrentStep() < SceneManager.GetInstance().currentLab.steps.Count - 1)
            {
                EventManager.OnTips(TipsType.Toast, "请完成实验所有操作步骤", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                {
                    FindObjectOfType<UITips>().OnDisTips();
                });
                return;
            }
            else
            {
                int currentStep = SceneManager.GetInstance().GetCurrentStep();
                //检查当前步骤是否有没填写的InputText
                if (ProxyManager.experimentInputProxy.experimentStepInputMap.ContainsKey(currentStep))
                {
                    if (!UIManager.GetInstance().UILabButton.uiLabReport.FinishedStepInput(ProxyManager.experimentInputProxy.experimentStepInputMap[currentStep]))
                    {
                        EventManager.OnTips(TipsType.Toast, "请完成实验所有操作步骤", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                        {
                            FindObjectOfType<UITips>().OnDisTips();
                        });
                        return;
                    }
                }
                if (!UIManager.GetInstance().UILabButton.uiLabReport.FinishedStepInput(new string[] { "IDInputField", "NameInputField", "ClassInputField" }))
                {
                    UIManager.GetInstance().UILabButton.uiLabReport.ShowFlashingImage(0);
                    return;
                }
            }
#endif
            string tipString = "提交实验报告";
            EventManager.OnTips(TipsType.Snackbar, tipString, () => { FindObjectOfType<UITips>().OnDisTips(); },
               () =>
               {
                   //添加完成的实验名字的记录
                   string labName = SceneManager.GetInstance().currentLab.labName;
                   //保存word数据
                   UIManager.GetInstance().UILabButton.uiLabReport.SaveData();
                   //做过实验标记为True
                   SceneManager.didExperiment = true;
                   //返回选择实验场景
                   UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
               });
        }

        /// <summary>
        /// 重新开始实验
        /// </summary>
        public void RestartTest()
        {
            EventManager.OnTips(TipsType.Snackbar, "重新开始", () => { FindObjectOfType<UITips>().OnDisTips(); },
             () =>
             {
                 SceneManager.didExperiment = true;
                 UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
             });
        }
    }
}