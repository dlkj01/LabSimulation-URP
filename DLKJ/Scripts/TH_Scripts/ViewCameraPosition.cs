using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCameraPosition : MonoBehaviour
{
    public Vector3 offect;
    public Transform target;
    void Update()
    {
        transform.position = target.position + offect;
    }
}
