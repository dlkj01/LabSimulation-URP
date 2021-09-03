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
        [SerializeField] private Text nameText;
        public Item item = null;
        public ViewType viewType;

        private int clickTimes = 0;
        private bool down = false;
        private bool drag = false;


        public void Initialized(Item item)
        {
            this.item = item;
            nameText.text = item.itemName;
            modelIcon.texture = item.icon;
        }

        public void SetView(ViewType type)
        {
            viewType = type;
        }

        public void OnPointerDown(PointerEventData data)
        {
         
            clickTimes++;
            down = true;
            if (item.libraryType != LibraryType.Wires)
            {
                modelIcon.texture = item.renderTexture;
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
            
            EventManager.OnMouseEnterItem(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //EventManager.OnScrollItem(this);
            // EventManager.OnMouseEnterItem(this.item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            clickTimes = 0;
            down = false;
            //drag = false;
            EventManager.OnScrollItem(null);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (down && !drag)
            {
                clickTimes = 0;
                EventManager.OnScrollItem(this);
                drag = true;
            }
        }

    }
}