using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DLKJ
{
    public class MathTest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log(MathTool.µ»–ßΩÿ√Ê(100, 0.5f, 8.5f, 0.2f));
            }
        }
    }
}