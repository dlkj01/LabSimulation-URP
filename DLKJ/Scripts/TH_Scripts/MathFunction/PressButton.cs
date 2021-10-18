using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    [SerializeField] private PressButtonGroup ButtonGroup;
    public bool isOn;
    [SerializeField] private float outPositionX;
    [SerializeField] private float inPositionX;
    public float Value;
    private void Awake()
    {
        if (ButtonGroup != null)
            ButtonGroup.pressButtons.Add(this);

    }
    public void OnClick()
    {
        if (ButtonGroup != null)
        {
            for (int i = 0; i < ButtonGroup.pressButtons.Count; i++)
            {
                if (ButtonGroup.pressButtons[i] != this)
                    ButtonGroup.pressButtons[i].SetActive(false);
            }
        }
        SetActive(true);
    }
    public virtual void SetActive(bool active)
    {
        isOn = active;
        transform.localPosition = isOn == true ? new Vector3(inPositionX, transform.localPosition.y, transform.localPosition.z) :
   new Vector3(outPositionX, transform.localPosition.y, transform.localPosition.z);
    }
    private void OnMouseDown()
    {
        OnClick();
    }

}
