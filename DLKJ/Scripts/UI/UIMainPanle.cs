using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
namespace DLKJ
{
    public class UIMainPanle : MonoBehaviour
    {
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float normalX;
        [SerializeField] private float targetX;
        [SerializeField] private Transform arrow;

        private bool isMoving;//正在移动？
        private bool isHide;//是否隐藏了
        RectTransform rectTF;
        private void Awake()
        {
            rectTF = transform as RectTransform;
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
            EventManager.OnTips(TipsType.Snackbar, "是否提交实验报告？", () => { FindObjectOfType<UITips>().OnDisTips(); },
               () =>
               {
                   //添加完成的实验名字的记录
                   string labName = SceneManager.GetInstance().currentLab.labName;
                   if (!UIManager.experimentID.Contains(labName))
                       UIManager.experimentID.Add(labName);
                   //保存word数据
                   UIManager.GetInstance().UILabButton.uiLabReport.SaveData();
                   //做过实验标记为True
                   SceneManager.didExperiment = true;
                   //返回选择实验场景
                   UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
               });
        }
    }
}