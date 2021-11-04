using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CustomImage : Image
{
    private PolygonCollider2D customCollider;
    protected override void Start()
    {
        base.Start();
        customCollider = GetComponent<PolygonCollider2D>();
    }
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return customCollider.OverlapPoint(screenPoint);
    }
}
