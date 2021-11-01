using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DLKJ
{


    public class UIItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    {
        public RawImage modelIcon;
        [SerializeField] public Text nameText;
        public Item item = null;
        public ViewType viewType;

        private int clickTimes = 0;
        private bool down = false;
        private bool drag = false;
        CameraPosData currentCameraPos;
        public void Initialized(Item item)
        {
            this.item = item;
            nameText.text = item.itemName;
            modelIcon.texture = item.icon;
            currentCameraPos = UI3DCamera.GetInstance.SetPosData(item.itemName);
        }
        public void OnStart()
        {
            UI3DCamera.GetInstance.InitDefaultPosition(currentCameraPos);
            EventManager.OnScrollItem(this);
        }

        public void SetView(ViewType type)
        {
            viewType = type;
        }

        public void OnPointerDown(PointerEventData data)
        {
            UI3DCamera.GetInstance.currentSelectName = nameText.text;
            clickTimes++;
            down = true;
            if (item.libraryType != LibraryType.Wires)
            {
                // modelIcon.texture = item.renderTexture;
                modelIcon.texture = UIManager.GetInstance()._3dCamera._3DCamera.targetTexture;
            }

            if (clickTimes >= 2)
            {
                EventManager.OnSelectedItem(this);
                EventManager.OnScrollItem(this);
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            down = false;
            drag = false;
            if (UI3DCamera.GetInstance.currentSelectName == nameText.text)
            {
                currentCameraPos = UI3DCamera.GetInstance.GetCurrentPos();
            }
            EventManager.OnMouseEnterItem(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponentInParent<ScrollRect>().vertical = false;
            UI3DCamera.GetInstance.InitDefaultPosition(currentCameraPos);
            EventManager.OnScrollItem(this);
            // EventManager.OnMouseEnterItem(this.item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            clickTimes = 0;
            down = false;
            drag = false;
            currentCameraPos = UI3DCamera.GetInstance.GetCurrentPos();
            GetComponentInParent<ScrollRect>().vertical = true;
            EventManager.OnScrollItem(null);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!drag)
            {
                clickTimes = 0;
                EventManager.OnScrollItem(this);
                drag = true;
            }
        }

    }
}