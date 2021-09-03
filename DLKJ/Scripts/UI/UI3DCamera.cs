using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace DLKJ
{
    public class UI3DCamera : MonoBehaviour
    {
        public Camera _3DCamera;

        public Vector3 pivotOffset = Vector3.zero;
        public float distance = 10.0f;
        public float minDistance = 2f;
        public float maxDistance = 15f;
        public float zoomSpeed = 1f;
        public float xSpeed = 250.0f;
        public float ySpeed = 250.0f;
        public bool allowYTilt = true;
        public float yMinLimit = -90f;
        public float yMaxLimit = 90f;
        private float x = 0.0f;
        private float y = 0.0f;
        private float targetX = 0f;
        private float targetY = 0f;
        public float targetDistance = 0f;

        public Transform pivot;
        private float xVelocity = 1f;
        private float yVelocity = 1f;
        private float zoomVelocity = 1f;
        private bool leftDown = false;


        

        private void Start()
        {
            var angles = transform.eulerAngles;
            targetX = x = angles.x;
            targetY = y = ClampAngle(angles.y, yMinLimit, yMaxLimit);
            targetDistance = distance;
        }

        private void OnEnable()
        {
            EventManager.OnScrollItemEvent += OnScrollItem;
        }

        private void OnDisable()
        {
            EventManager.OnScrollItemEvent -= OnScrollItem;
        }

        void OnScrollItem(UIItem uIItem)
        {
            if (uIItem == null)
            {
                pivot = null;

            }
            else
            {
                _3DCamera.targetTexture = uIItem.item.renderTexture;
                GameObject target = SceneManager.GetInstance().GetItemObject(uIItem.item);
                target.SetActive(true);
                pivot = target.transform;
                uIItem.modelIcon.texture = _3DCamera.targetTexture;
            }
        }


        private void LateUpdate()
        {

            if (!pivot) return;
            var scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0.0f)
            {
                targetDistance -= zoomSpeed;
            }
            else if (scroll < 0.0f)
            {
                targetDistance += zoomSpeed;
            }

            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);



            if (Input.GetMouseButtonDown(0))
            {
                leftDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                leftDown = false;
            }

            if (leftDown)
            {
                targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                if (allowYTilt)
                {
                    targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                    targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
                }
            }

            x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0f);
            y = allowYTilt ? Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0f) : targetY;
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0f);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position + pivotOffset;
            transform.rotation = rotation;
            transform.position = position;
        }


        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }


        //ÖÆ×÷RenderTexture
        public static void DumpRenderTexture(RenderTexture rt, string pngOutPath)
        {
            Debug.Log("Name:" + rt.name);
            var oldRT = RenderTexture.active;

            var tex = new Texture2D(rt.width, rt.height);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
           // File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());

            FileStream file = File.Open(pngOutPath + "/" + "RenderTexture.renderTexture", FileMode.Create);
            var binary = new BinaryWriter(file);
            binary.Write(bytes);
            binary.Flush();
            binary.Close();
            file.Close();

           // RenderTexture.active = oldRT;
             //AssetDatabase.Refresh();
        }

    }

}