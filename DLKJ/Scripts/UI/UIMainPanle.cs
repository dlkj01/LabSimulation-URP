using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DLKJ
{
    public class UIMainPanle : MonoBehaviour
    {
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float normalX;
        [SerializeField] private float targetX;
        [SerializeField] private Transform arrow;

        private bool isMoving;//�����ƶ���
        private bool isHide;//�Ƿ�������
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
            if (SceneManager.GetInstance().VerifyBasicLink())
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
            EventManager.OnTips(TipsType.Snackbar, "�Ƿ��ύʵ�鱨�棿", () => { FindObjectOfType<UITips>().OnDisTips(); },
               () =>
               {
                   UIManager.GetInstance().UILabButton.uiLabReport.SaveData();
               });
        }
    }
}