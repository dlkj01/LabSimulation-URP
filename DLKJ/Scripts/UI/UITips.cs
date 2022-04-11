using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace DLKJ
{
    public enum TipsType
    {
        None,
        Toast,          //提示框(仅显示文字)
        Dialog,         //对话框
        Snackbar,       //提示对话框
        Actionbar,      //功能框
        DialogWithIcon, //带图片的对话框
        NetWorkRequest, //网络连接请求
    }

    public class UITips : MonoBehaviour
    {
        public Text tipsContent;
        public Button sureButton;
        public Button cancelButton;
        public RectTransform parent;

        private UnityAction noUnityAction;
        private UnityAction yesUnityAction;

        private void OnEnable()
        {
            EventManager.OnTipsEvent += OnTips;
            EventManager.OnTipsDecidedEvent += Decided;
        }

        private void OnDisable()
        {
            EventManager.OnTipsEvent -= OnTips;
            EventManager.OnTipsDecidedEvent -= Decided;
        }

        public void OnTips(TipsType type, string content, UnityAction noCallback = null, UnityAction yesCallback = null)
        {
            noUnityAction = noCallback;
            yesUnityAction = yesCallback;

            if (type == TipsType.None)
            {

            }
            else if (type == TipsType.Toast)
            {
                SetSelectedButtonState(false);
                //parent.pivot = new Vector2(0.5f, 0.5f);
                tipsContent.transform.localPosition = Vector3.zero;
                Init(content);
            }
            else if (type == TipsType.Dialog)
            {
                SetSelectedButtonState(true);
                Init(content);
            }
            else if (type == TipsType.Snackbar)
            {
                Init(content);
                SetSelectedButtonState(false);
                cancelButton.gameObject.SetActive(true);
                if (noUnityAction != null)
                {
                    cancelButton.onClick.AddListener(noUnityAction);
                }
                else
                {
                    cancelButton.onClick.AddListener(Cancel);
                }
            }
            else if (type == TipsType.Actionbar)
            {

            }
            else if (type == TipsType.DialogWithIcon)
            {
                SetSelectedButtonState(true);
                Init(content);
            }
        }


        public void Init(string content)
        {
            tipsContent.text = content;//RenderTXTWithColor(content,Color.red);
            OnTips();
        }

        public void Cancel()
        {
            //EventManager.OnTipsNoBtn();
        }

        public void Decided()
        {
            OnDisTips();
        }

        public void OnTips()
        {
            transform.localPosition = Vector3.zero;
            transform.SetAsLastSibling();
            Debug.Log("置顶弹窗");
        }

        public void OnDisTips()
        {
            transform.localPosition = new Vector3(9999, 9999, 9999);
            transform.SetAsFirstSibling();
        }

        private string RenderTXTWithColor(string keyword, Color color)
        {
            string subStr = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + keyword + "</color>";
            return subStr;
        }

        private float GetTxtContentHeight(string content)
        {
            return tipsContent.preferredHeight;
        }



        private void SetSelectedButtonState(bool state)  //由于多次添加按钮监听所以导致事件多次被触发 应注意
        {
            sureButton.gameObject.SetActive(!state);
            cancelButton.gameObject.SetActive(state);


            sureButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            if (noUnityAction != null)
            {
                cancelButton.onClick.AddListener(noUnityAction);
            }
            else
            {
                cancelButton.onClick.AddListener(Cancel);
            }

            if (yesUnityAction != null)
            {
                sureButton.onClick.AddListener(yesUnityAction);
            }
            else
            {
                sureButton.onClick.AddListener(Decided);
            }
        }
    }
}