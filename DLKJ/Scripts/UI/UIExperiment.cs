using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Common;
using T_Common;
namespace DLKJ
{
    public class UIExperiment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Text titleText;
        [SerializeField] Text experimentCountText;//实验得分
        [SerializeField] Color defaultColor;
        [SerializeField] Color selectedColor;
        [SerializeField] Button button;
        [SerializeField] Image image;
        [SerializeField] Image selected;
        [SerializeField] Image scoreBackground;
        CanvasGroup group;
        private Lab lab;
        private float targetScale = 1.05f;

        private Coroutine toLarge;
        private Coroutine toNormal;
        private void OnEnable()
        {
         
        }

        public void Awake()
        {
            scoreBackground.gameObject.SetActive(false);
            group = GetComponent<CanvasGroup>();
            selected.gameObject.SetActive(false);
            if (button) button.onClick.AddListener(delegate
            {
                LabSelectedCallBack(); UI3DCamera.GetInstance.OnStart();
                button.interactable = false;
            });
        }
        private void Start()
        {
            switch (transform.FindChildByName("Text").GetComponent<Text>().text)
            {
                case SceneManager.FIRST_EXPERIMENT_NAME:
                    transform.FindChildByName("ScoreRate").GetComponent<Text>().text = "总分占比:40%";
                    break;
                case SceneManager.SECOND_EXPERIMENT_NAME:
                    transform.FindChildByName("ScoreRate").GetComponent<Text>().text = "总分占比:50%";
                    break;
                case SceneManager.THIRD_EXPERIMENT_NAME:
                    transform.FindChildByName("ScoreRate").GetComponent<Text>().text = "总分占比:10%";
                    break;
            }
        }

        public void Initialized(Lab lab)
        {
            this.lab = lab;
            image.sprite = lab.icon;

            if (titleText) titleText.text = lab.labName;

        }

        public void SetUnInteractive()
        {
            group.blocksRaycasts = false;
            group.interactable = false;
            group.alpha = 0.5f;
            string score = ProxyManager.saveProxy.map[titleText.text].score.ToString();
            Debug.Log(titleText.text + "得分："+score);
            if (score.Contains("."))
            {
                experimentCountText.text = ProxyManager.saveProxy.map[titleText.text].score.ToString("#0.00");
            }
            else
            {
                experimentCountText.text = ProxyManager.saveProxy.map[titleText.text].score.ToString();
            }
            scoreBackground.gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            selected.gameObject.SetActive(true);
            titleText.color = selectedColor;
            if (toLarge != null) StopCoroutine(toLarge);
            toLarge = StartCoroutine(ToLarge());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            selected.gameObject.SetActive(false);
            titleText.color = defaultColor;
            if (toNormal != null) StopCoroutine(toNormal);
            toNormal = StartCoroutine(ToNormal());
        }

        IEnumerator ToLarge()
        {
            float value = 1;
            while (transform.localScale.x < targetScale)
            {
                value += Time.deltaTime * 0.5f;
                if (value > targetScale) value = targetScale;
                transform.localScale = new Vector3(value, value, value);

                yield return null;
            }
        }

        IEnumerator ToNormal()
        {
            float value = targetScale;
            while (transform.localScale.x > Vector3.one.x)
            {
                value -= Time.deltaTime * 0.5f;
                if (value < 1) value = 1;
                transform.localScale = new Vector3(value, value, value);
                yield return null;

            }
        }

        void LabSelectedCallBack()
        {
            lab.Initialized();
            UIManager.GetInstance().ShowEquipmentPanel(lab.ID);
         
            ProxyManager.InitProxy(new ExperimentInputVerifyProxy("ExperimentInputVerify"));
        }

    }
}