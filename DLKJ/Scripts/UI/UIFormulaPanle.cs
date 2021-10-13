using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFormulaPanle : MonoBehaviour
{
    public RectTransform FormulaPanle;
    bool isOpen = false;

    public float maxMoveValue;
    public float minMoveValue;

    public void FormulaButton()
    {
        isOpen = !isOpen;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(MoveImage());
    }
    Coroutine coroutine;
    IEnumerator MoveImage()
    {
        if (!isOpen)
        {
            while (FormulaPanle.anchoredPosition.x <= maxMoveValue)
            {
                FormulaPanle.anchoredPosition = new Vector2(FormulaPanle.anchoredPosition.x + 20, FormulaPanle.anchoredPosition.y);
                yield return null;
            }
            FormulaPanle.anchoredPosition = new Vector2(maxMoveValue, FormulaPanle.anchoredPosition.y);

        }
        else
        {
            while (FormulaPanle.anchoredPosition.x >= minMoveValue)
            {
                FormulaPanle.anchoredPosition = new Vector2(FormulaPanle.anchoredPosition.x - 20, FormulaPanle.anchoredPosition.y);
                yield return null;
            }
            FormulaPanle.anchoredPosition = new Vector2(minMoveValue, FormulaPanle.anchoredPosition.y);
        }
    }
}
