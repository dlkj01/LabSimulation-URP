using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILabData : MonoBehaviour
{
    [SerializeField] Text title;  //������
    [SerializeField] Text value;  //����ֵ

    public string Key { get { return title.text; } }
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetValue(double targetValue)
    {
        if (value) value.text = targetValue.ToString();
    }
}
