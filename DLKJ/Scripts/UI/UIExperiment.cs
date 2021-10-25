using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace DLKJ
{
    public class UIExperiment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Text titleText;
        [SerializeField] Text experimentCountText;//第几组实验?
        [SerializeField] Color defaultColor;
        [SerializeField] Color selectedColor;
        [SerializeField] Button button;
        [SerializeField] Image image;
        [SerializeField] Image selected;
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
            group = GetComponent<CanvasGroup>();
            selected.gameObject.SetActive(false);
            if (button) button.onClick.AddListener(delegate { LabSelectedCallBack(); });
        }


        public void Initialized(Lab lab)
        {
            this.lab = lab;
            image.sprite = lab.icon;

            if (titleText) titleText.text = lab.labName;

            if (titleText.text == "负载阻抗测量")
            {
                switch (SceneManager.experimentCount)
                {
                    case 0:
                        experimentCountText.text = "开始第一组实验";
                        break;
                    case 1:
                        experimentCountText.text = "开始第二组实验";
                        break;
                    default:
                        experimentCountText.text = "";
                        break;
                }
            }
            for (int i = 0; i < UIManager.experimentID.Count; i++)
            {
                if (UIManager.experimentID[i] == lab.labName)
                {
                    if (lab.labName == "负载阻抗测量")
                    {
                        if (SceneManager.experimentCount >= 2)
                        {
                            group.blocksRaycasts = false;
                            group.interactable = false;
                            group.alpha = 0.5f;
                        }
                    }
                    else
                    {
                        group.blocksRaycasts = false;
                        group.interactable = false;
                        group.alpha = 0.5f;
                    }
                }
            }

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
        }

    }
}