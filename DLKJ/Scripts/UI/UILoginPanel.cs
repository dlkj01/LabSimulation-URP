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
            if (sureButton) sureButton.onClick.AddListener(delegate() { SureCallBack(); });
            if (exitButton) exitButton.onClick.AddListener(delegate() { ExitCallBack(); });
        }

        void SureCallBack()
        {
            if (nameInputField.text.Length>0)
            {

            }
            else
            {
                errorInfoText.gameObject.SetActive(true);
                errorInfoText.text = "�������û���";
                return;
            }

            if (codeInputField.text.Length>0)
            {

            }
            else
            {
                errorInfoText.gameObject.SetActive(true);
                errorInfoText.text = "�������û�����";
                return;
            }

            UIManager.GetInstance().ShowExperimentSelectedPanel();
            gameObject.SetActive(false);
        }

        void ExitCallBack()
        {
            Application.Quit();
        }

    }
}