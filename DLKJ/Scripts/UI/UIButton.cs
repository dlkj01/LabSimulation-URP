using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DLKJ
{
    public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Sprite normal;
        [SerializeField] Sprite highlight;
        [SerializeField] Sprite selected;
        [SerializeField] Sprite disable;
        [SerializeField] public Button button;

        public Color textNormalColor;
        public Color textHiglightColor;
        public Color textInteractableColor;
        public Text buttonText;
        void Awake()
        {
            if (button == null) button = transform.GetComponent<Button>();

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (highlight) button.image.sprite = highlight;
            if (buttonText != null)
               if(button.interactable) buttonText.color = textHiglightColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (normal) button.image.sprite = normal;
            if (buttonText != null)
                if (button.interactable) buttonText.color = textNormalColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (selected) button.image.sprite = selected;
        }


        public void Interactable(bool state)
        {
            button.interactable = state;
            if (state)
            {
                buttonText.color = textNormalColor;
            }
            else
            {
                buttonText.color = textInteractableColor;
            }
        }

    }
}
