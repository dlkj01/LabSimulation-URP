using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButtonGroup : MonoBehaviour
{
    [HideInInspector] public List<PressButton> pressButtons = new List<PressButton>();
    public float currentValue;
    public PressButton startButton;
    private void Start()
    {
        startButton.OnClick();
        currentValue = GetValue();
    }

    public float GetValue()
    {
        foreach (var item in pressButtons)
        {
            if (item.isOn)
            {
                currentValue = item.Value;
                return item.Value;
            }
        }
        return 1;
    }
}
