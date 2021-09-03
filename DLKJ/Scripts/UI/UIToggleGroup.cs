using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DLKJ
{
    public class UIToggleGroup : MonoBehaviour
    {
        [SerializeField] float offsetY = -3.4f;
        [SerializeField] Sprite selected;
        [SerializeField] Sprite normal;
        [SerializeField] Color selectedColor;
        [SerializeField] Color normalColor;
        

        [SerializeField] Toggle equip;
        [SerializeField] Text equipText;
        [SerializeField] Toggle device;
        [SerializeField] Text deviceText;
        [SerializeField] GameObject equipScrollView;
        [SerializeField] GameObject deviceScrollView;


        public void ToogleCallback()
        {
            if (equip.isOn)
            {
                equipText.color = selectedColor;
                equip.image.sprite = selected;
                equip.image.SetNativeSize();
                equip.image.transform.localPosition = new Vector3(equip.image.transform.localPosition.x,0, equip.image.transform.localPosition.z);

                deviceText.color = normalColor;
                device.image.sprite = normal;
                device.image.SetNativeSize();
                device.image.transform.localPosition = new Vector3(device.image.transform.localPosition.x, offsetY, device.image.transform.localPosition.z);

                equip.transform.SetAsLastSibling();
                equipScrollView.gameObject.SetActive(equip.isOn);
                deviceScrollView.gameObject.SetActive(!equip.isOn);
            }
            else
            {
                deviceText.color = selectedColor;
                device.image.sprite = selected;
                device.image.SetNativeSize();
                device.image.transform.localPosition = new Vector3(device.image.transform.localPosition.x, 0, device.image.transform.localPosition.z);

                equipText.color = normalColor;
                equip.image.sprite = normal;
                equip.image.SetNativeSize();
                equip.image.transform.localPosition = new Vector3(equip.image.transform.localPosition.x, offsetY, equip.image.transform.localPosition.z);

                device.transform.SetAsLastSibling();
                equipScrollView.gameObject.SetActive(equip.isOn);
                deviceScrollView.gameObject.SetActive(!equip.isOn);
            }
        }


    }
}