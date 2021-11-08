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
        private bool isMoving;//�����ƶ���
        private bool isHide;//�Ƿ�������
        RectTransform rectTF;
        public UIButton autoConnect;
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
                EventManager.OnTips(TipsType.Toast, "����ɻ�������");
            }
        }

        public void ViewLabReport()
        {
            UIManager.GetInstance().UILabButton.uiLabReport.SetVisibale(true);
        }

        public void SubmitTest()
        {
            if (SceneManager.GetInstance().GetCurrentStep() < SceneManager.GetInstance().currentLab.steps.Count - 1)
            {
                EventManager.OnTips(TipsType.Toast, "�����ʵ�����в�������", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                {
                    FindObjectOfType<UITips>().OnDisTips();
                });
                return;
            }
            else
            {
                int currentStep = SceneManager.GetInstance().GetCurrentStep();
                //��鵱ǰ�����Ƿ���û��д��InputText
                if (ProxyManager.experimentInputProxy.experimentStepInputMap.ContainsKey(currentStep))
                {
                    if (!UIManager.GetInstance().UILabButton.uiLabReport.FinishedStepInput(ProxyManager.experimentInputProxy.experimentStepInputMap[currentStep]))
                    {
                        EventManager.OnTips(TipsType.Toast, "�����ʵ�����в�������", () => { FindObjectOfType<UITips>().OnDisTips(); }, () =>
                        {
                            FindObjectOfType<UITips>().OnDisTips();
                        });
                        return;
                    }
                }
                if (!UIManager.GetInstance().UILabButton.uiLabReport.FinishedStepInput(new string[] { "IDInputField" }))
                {
                    UIManager.GetInstance().UILabButton.uiLabReport.ShowPanle(false, false);
                    return;
                }
            }


            string tipString = "�ύʵ�鱨��";
            EventManager.OnTips(TipsType.Snackbar, tipString, () => { FindObjectOfType<UITips>().OnDisTips(); },
               () =>
               {
                   //�����ɵ�ʵ�����ֵļ�¼
                   string labName = SceneManager.GetInstance().currentLab.labName;
                   //����word����
                   UIManager.GetInstance().UILabButton.uiLabReport.SaveData();
                   //����ʵ����ΪTrue
                   SceneManager.didExperiment = true;
                   //����ѡ��ʵ�鳡��
                   UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
               });
        }

        /// <summary>
        /// ���¿�ʼʵ��
        /// </summary>
        public void RestartTest()
        {
            EventManager.OnTips(TipsType.Snackbar, "���¿�ʼ", () => { FindObjectOfType<UITips>().OnDisTips(); },
             () =>
             {
                 SceneManager.didExperiment = true;
                 UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
             });
        }
    }
}