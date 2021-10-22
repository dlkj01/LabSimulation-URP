using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeFlow : MonoBehaviour
{

    public FlowItem ItemTemplate;
    public float FlowTime = 1f;
    public float FlowInterval = 0.5f;
    public bool OnlyOnce = false;

    [HideInInspector]
    public List<Vector3> FlowPath = new List<Vector3>();
    [HideInInspector]
    public List<float> FlowSpeeds = new List<float>();
    [HideInInspector]
    public List<FlowItem> Items = new List<FlowItem>();

    private bool _isInit = false;
    private bool _isFlowing = false;
    private float _flowInterval = 0f;
    private Action _actionTrigger;
    private Coroutine _actionCoroutine;

    private void Awake()
    {
        if (!_isInit)
        {
            Init();
        }
    }

    private void Init()
    {
        float tatol = 0f;
        for (int i = 0; i < FlowPath.Count - 1; i++)
        {
            tatol += Vector3.Distance(FlowPath[i], FlowPath[i + 1]);
        }
        for (int i = 0; i < FlowPath.Count - 1; i++)
        {
            float dis = Vector3.Distance(FlowPath[i], FlowPath[i + 1]);
            float time = dis / tatol * (FlowTime * 50);
            FlowSpeeds.Add(1f / time);
        }

        _isInit = true;
    }

    private void Update()
    {
        if (_isFlowing)
        {
            _flowInterval += Time.deltaTime;
            if (_flowInterval >= FlowInterval)
            {
                _flowInterval = 0f;
                ShootItem();

                if (OnlyOnce)
                {
                    _isFlowing = false;
                }
            }
        }
    }

    private void ShootItem()
    {
        if (Items.Count > 0)
        {
            Items[0].Shoot(this);
            Items.RemoveAt(0);
        }
        else
        {
            GameObject item = Instantiate(ItemTemplate.gameObject);
            item.transform.parent = transform;
            item.GetComponent<FlowItem>().Shoot(this);
        }
    }

    public void Flow(Action endAction)
    {
        if (FlowPath.Count < 2)
        {
            Debug.LogWarning("·��������������ڵ���2��");
            return;
        }
        if (!ItemTemplate)
        {
            Debug.LogWarning("ItemTemplate����Ϊ�գ�");
            return;
        }

        if (!_isInit)
        {
            Init();
        }

        _isFlowing = true;
        _flowInterval = FlowInterval;

        _actionTrigger = endAction;
        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }
        if (_actionTrigger != null)
        {
            _actionCoroutine = StartCoroutine(DelayExecute(_actionTrigger, FlowTime));
        }
    }

    public void Stop()
    {
        _isFlowing = false;

        if (_actionCoroutine != null)
        {
            StopCoroutine(_actionCoroutine);
            _actionCoroutine = null;
        }

        FlowItem[] fis = transform.GetComponentsInChildren<FlowItem>();
        foreach (FlowItem fi in fis)
        {
            fi.Stop();
        }
    }

    public static IEnumerator DelayExecute(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}

