using DLKJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebLine : MonoBehaviour
{
    public LineRenderer line;
    [HideInInspector] public Transform startPos;
    [HideInInspector] public Transform endPos;
    public string Name { get { return GetComponent<Item>().itemName; } }
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();
        //禁用原来的线，用现在的Line
#if UNITY_WEBGL || UNITY_EDITOR
        ChangeLine();
#elif UNITY_STANDALONE
        enabled = false;
       line.enabled = false;
#else
#endif
    }
    // Update is called once per frame
    void Update()
    {
        if (!line || !startPos || !endPos) return;
        line.SetPosition(0, startPos.position);
        line.SetPosition(1, endPos.position);
    }
    private void ChangeLine()
    {
        line = GetComponent<LineRenderer>();
        if (line == null)
            line = gameObject.AddComponent<LineRenderer>();
        line.SetWidth(0.008f, 0.008f);
        line.SetColors(Color.black, Color.black);
        line.generateLightingData = true;
        line.enabled = true;
    }
    public void SetLineRender(Transform startTF, Transform endTF)
    {
        startPos = startTF;
        endPos = endTF;
    }
}
