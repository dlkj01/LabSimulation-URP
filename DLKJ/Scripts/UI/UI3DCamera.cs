using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Common;
namespace DLKJ
{
    public struct CameraPosData
    {
        public float X;
        public float Y;
        public float distance;
    }
    public class UI3DCamera : MonoSingleton<UI3DCamera>
    {
        public string currentSelectName;
        public bool isInput = false;
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
        [SerializeField] private float x = 0.0f;
        [SerializeField] private float y = 0.0f;
        private float targetX = 0f;
        private float targetY = 0f;
        public float targetDistance = 0f;

        public Transform pivot;
        private float xVelocity = 1f;
        private float yVelocity = 1f;
        private float zoomVelocity = 1f;
        private bool leftDown = false;


        private Dictionary<string, CameraPosData> map = new Dictionary<string, CameraPosData>();

        private void Awake()
        {
            var angles = transform.eulerAngles;
            targetX = x = angles.x;
            targetY = y = ClampAngle(angles.y, yMinLimit, yMaxLimit);
            targetDistance = distance;
            Sheet1ExcelData data = ExcelManager.GetInstance.GetExcelData<Sheet1ExcelData, Sheet1ExcelItem>();
            foreach (var item in data.items)
                map.Add(item.id, new CameraPosData() { distance = item.Distance, X = item.X, Y = item.Y });
        }
        public void OnStart()
        {
            UIItem[] uiItems = FindObjectOfType<UIEquipmentPanel>().uIItems.ToArray();
            StartCoroutine(WaitForView(uiItems));
        }
        IEnumerator WaitForView(UIItem[] uiItems)
        {
            for (int i = 0; i < uiItems.Length; i++)
            {
                yield return new WaitForEndOfFrame();
                uiItems[i].OnStart();
            }
        }
        public CameraPosData GetCurrentPos()
        {
            return new CameraPosData()
            {
                distance = targetDistance,
                X = targetX,
                Y = targetY
            };
        }
        public CameraPosData SetPosData(string deviceName)
        {
            if (map.ContainsKey(deviceName))
                return map[deviceName];
            return default;
        }
        public void InitDefaultPosition(CameraPosData pos)
        {
            targetX = x = pos.X;
            targetY = y = ClampAngle(pos.Y, yMinLimit, yMaxLimit);
            targetDistance = distance = Mathf.Clamp(pos.distance, minDistance, maxDistance);
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
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                OnStart();
            }
        }
        public void LateUpdate()
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