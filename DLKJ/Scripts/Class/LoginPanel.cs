using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class LoginPanel : MonoBehaviour
    {
        public static ParaObject paraObject = new ParaObject();
        public static Stepspro stepspro = new Stepspro();
        [SerializeField]
        private InputField userName;
        [SerializeField]
        private Transform errorText;

        private void Awake()
        {
            errorText.gameObject.SetActive(false);
        }

        public void Login()
        {
           
            if (userName.text == "")
            {
                errorText.gameObject.SetActive(true);
                return;
            }
            errorText.gameObject.SetActive(false);
            paraObject.username = userName.text;
            paraObject.startTime = TimeHelp.GetTimeStampuse(DateTime.Now);
            transform.gameObject.SetActive(false);
            Debug.Log("登录成功" + "用户名"+paraObject.username+"开始时间"+ paraObject.startTime);

        }

        public void Exit()
        {
            //UnityEditor.EditorApplication.isPlaying = false;    
        }


     
    }

}

