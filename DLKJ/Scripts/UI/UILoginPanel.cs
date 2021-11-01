using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DLKJ
{
    public class UILoginPanel : MonoBehaviour
    {
        [SerializeField] private Text errorInfoText;
        [SerializeField] private InputField nameInputField;
        [SerializeField] private InputField codeInputField;
        [SerializeField] private Button sureButton;
        [SerializeField] private Button exitButton;

        public void Awake()
        {
            errorInfoText.gameObject.SetActive(false);
            UIManager.GetInstance().SetVerifyButtonActive(false);
            if (sureButton) sureButton.onClick.AddListener(delegate () { SureCallBack(); });
            if (exitButton) exitButton.onClick.AddListener(delegate () { ExitCallBack(); });
            if (SceneManager.didExperiment == true)
            {
                UIManager.GetInstance().ShowExperimentSelectedPanel();
                gameObject.SetActive(false);
            }
        }

        void SureCallBack()
        {
            if (nameInputField.text.Length > 0)
            {
                int result = ExcelRead.GetInstance.Verify(nameInputField.text, codeInputField.text);
                if (result == 0)
                {
                    errorInfoText.gameObject.SetActive(true);
                    errorInfoText.text = "没有此用户";
                }
                else if (result == 1)
                {
                    errorInfoText.gameObject.SetActive(true);
                    errorInfoText.text = "密码不正确";
                }
                else
                {
                    UIManager.GetInstance().ShowExperimentSelectedPanel();
                    gameObject.SetActive(false);
                    UserData data = ExcelRead.GetInstance.GetUserData(nameInputField.text);
                    SceneManager.loginUserData = data;
                    UIManager.GetInstance().uiMainPanle.Init(data.userType);
                }
            }
            else
            {
                errorInfoText.gameObject.SetActive(true);
                errorInfoText.text = "请输入用户名";
                return;
            }
        }

        void ExitCallBack()
        {
            Application.Quit();
        }

    }
}