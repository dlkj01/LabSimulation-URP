using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILabReportBase : MonoBehaviour
{
    public GameObject page1, page2;
    protected CanvasGroup canvasGroup;
    private Coroutine coroutine;
    bool isPlaying = false;
    private float fadeSpeed = 5;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    public void SetVisibale(bool state)
    {
        if (canvasGroup.alpha == 0 && state == false) return;
        if (canvasGroup.alpha == 1 && state == true) return;
        if (isPlaying == true)
            return;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(SetVisibleDelay(state));
    }
    public void ChangePage()
    {
        bool state = page1.activeSelf;
        page1.SetActive(!state);
        page2.SetActive(state);
    }
    IEnumerator SetVisibleDelay(bool state)
    {
        canvasGroup.alpha = state == false ? 1 : 0;
        isPlaying = true;
        if (state == true)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }
            isPlaying = false;
        }
        else
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
            isPlaying = false;
        }
        if (canvasGroup.alpha >= 1)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }

    }
}
