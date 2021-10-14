using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UILabReportController : MonoBehaviour
{
    [HideInInspector] public UILabReportBase uiLabReport;
    CanvasGroup group;
    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        UIEventListener listener = UIEventListener.GetUIEventListener(gameObject);
        listener.PointerClick += OnPointerClick;
        listener.PointerEnter += OnPointerEnter;
        listener.PointerExit += OnPointerExit;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        uiLabReport.SetVisibale(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        group.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        group.alpha = 0.6f;
    }
}
