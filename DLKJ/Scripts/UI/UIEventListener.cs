using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public delegate void OnPointerClickHandler(PointerEventData eventData);
public delegate void OnPointerEnterHandler(PointerEventData eventData);
public delegate void OnPointerExitHandler(PointerEventData eventData);
public class UIEventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event OnPointerClickHandler PointerClick;
    public event OnPointerEnterHandler PointerEnter;
    public event OnPointerExitHandler PointerExit;
    public static UIEventListener GetUIEventListener(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null)
            listener = go.AddComponent<UIEventListener>();
        return listener;
    }
    public void OnPointerClick(PointerEventData eventData) => PointerClick?.Invoke(eventData);

    public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke(eventData);

    public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke(eventData);
}
