using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowDatas : MonoBehaviour
{
    [SerializeField] bool isShow = true;
    [SerializeField] public List<UILabData> datas = new List<UILabData>();
    private void Start()
    {
        gameObject.SetActive(isShow);
    }

    public void UpdateDatas(string key, double target1)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].Key == key)
            {
                datas[i].SetValue(target1);
                break;
            }
        }
    }


}
