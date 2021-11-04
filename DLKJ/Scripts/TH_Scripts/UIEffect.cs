using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class UIEffect : MonoBehaviour
{
    Image image;
    Outline outline;
    Tweener tween;
    private Color startColor = new Color(1, 0, 0, 0);

    void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
            image = gameObject.AddComponent<Image>();
        outline = GetComponent<Outline>();
        if (outline == null)
            outline = gameObject.AddComponent<Outline>();
        image.raycastTarget = false;
        outline.effectColor = startColor;
        outline.effectDistance = new Vector2(3, 3);
    }
    public void StartFlashing()
    {
        tween.Kill();
        outline.effectColor = startColor;
        tween = outline.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void StopFlashing()
    {
        tween.Kill();
        outline.effectColor = startColor;
    }
}
