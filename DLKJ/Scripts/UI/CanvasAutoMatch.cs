using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using DLKJ;

namespace DLKJ
{

    public class CanvasAutoMatch : MonoBehaviour
    {

        public bool autoMatch = false;

        void Awake()
        {
            if (autoMatch)
            {
                StartCanvasMatch();
            }
        }

        public void CanvasMatch()
        {
            StartCoroutine("CanvasMatchDelay");
        }

        public IEnumerator CanvasMatchDelay()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            StartCanvasMatch();
        }

        public void StartCanvasMatch()
        {
            int width = Screen.width;
            int height = Screen.height;
            Debug.Log("width / height = " + (float)width / height);
            if ((float)width / height >= 16f / 9f)
            {
                CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = 1.0f;
            }
            else
            {
                CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = 0.0f;
            }
        }
    }
}
