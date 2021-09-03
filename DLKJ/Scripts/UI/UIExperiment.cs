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
        [SerializeField] Color defaultColor;
        [SerializeField] Color selectedColor;
        [SerializeField] Button button;
        [SerializeField] Image image;
        [SerializeField] Image selected;
        private Lab lab;
        private float targetScale = 1.05f;

        private Coroutine toLarge;
        private Coroutine toNormal;


        public void Awake()
        {
            selected.gameObject.SetActive(false);
            if (button) button.onClick.AddListener(delegate { LabSelectedCallBack(); });
        }


        public void Initialized(Lab lab)
        {
            this.lab = lab;
            image.sprite = lab.icon;

            if (titleText) titleText.text = lab.labName;
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
            while (transform.localScale.x >Vector3.one.x)
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