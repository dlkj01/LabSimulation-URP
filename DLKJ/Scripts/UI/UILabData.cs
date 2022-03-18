using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILabData : MonoBehaviour
{
    [SerializeField] Text title;  //������
    [SerializeField] Text value;  //����ֵ

    public string Key { get { return title.text.ToString(); } }
 
    void Start()
    {
        Debug.Log("title.text.ToString():" + title.text.ToString());

    }

    public void SetValue(double targetValue)
    {
        if (value) value.text = targetValue.ToString();
    }
}
