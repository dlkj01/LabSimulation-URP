
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DLKJ.InstrumentAction;
using System.Web;
using static DLKJ.InstrumentAction.InstrumentButton;

namespace DLKJ
{

    public class MoveCamera : MonoBehaviour
    {

        private Vector3 dirVector3;
        private float paramater = 0.5f;
        public float sensitivityX = 2F;
        public float sensitivityY = 2F;
        float rotationY = 0F;
        public float minimumY = -90F;
        public float maximumY = 90F;
        public float speed = 1f;
        public float lagerspeed = 3f;
        public float distance = 10f;

        private void Awake()
        {
            Application.targetFrameRate = 120;
        }

        public InstrumentButton CamRayCast()
        {
            Ray m_ray;
            RaycastHit m_hit;
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_ray, out m_hit))
            {

                //if (m_hit.collider.gameObject.GetComponent<Item>() != null)
                //{
                //    Rigidbody rigidbody = m_hit.collider.gameObject.AddComponent<Rigidbody>();
                //    rigidbody.isKinematic = true;
                //}
                InstrumentAction instrumentAction;
                InstrumentButton curInstrumentButton;
                if (m_hit.collider.transform.root.TryGetComponent<InstrumentAction>(out instrumentAction))
                {
                    curInstrumentButton = instrumentAction.instrumentButton.Find(x => x.instrumentButton.name == m_hit.collider.transform.name);
                    return curInstrumentButton;
                    //Debug.Log(m_hit.collider.transform.name);

                }

            }
            return null;
        }
        private float interval = 0.02f;
        private float currentTime;
        private float startHoldTime = 1f;
        private float startClickTime;
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CamRayCast() != null)
                {
                    CamRayCast().OnMouseClick();
                }
            }
            if (Input.GetMouseButton(0))
            {
                startClickTime += Time.deltaTime;
                if (startClickTime > startHoldTime)
                {
                    currentTime += Time.deltaTime;
                    if (currentTime >= interval)
                    {
                        currentTime = 0;
                        InstrumentButton button = CamRayCast();
                        if (button != null)
                        {
                            if (button.instrumentButtonType == InstrumentButtonType.Rotary)
                            {
                                CamRayCast().OnMouseClick();
                            }
                        }

                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                startClickTime = 0;
                currentTime = 0;
            }

            RotationCamera();
            dirVector3 = Vector3.zero;
            KeyBoardMove();
            //CameraLimit();
        }
        /// <summary>
        /// 摄像机在屋内移动
        /// </summary>
        private void CameraLimit()
        {
            if (transform.position.z <= 2.2f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 2.2f);
            }
            if (transform.position.z >= 13.4f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 13.4f);
            }
            if (transform.position.x >= 4.4f)
            {
                transform.position = new Vector3(4.4f, transform.position.y, transform.position.z);
            }
            if (transform.position.x <= -4.7f)
            {
                transform.position = new Vector3(-4.7f, transform.position.y, transform.position.z);
            }
            if (transform.position.y <= 1f)
            {
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
            }
            if (transform.position.y >= 3f)
            {
                transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
            }
        }
        /// <summary>
        ///WASDQE 键盘移动摄像头   
        /// </summary>
        private void KeyBoardMove()
        {
            dirVector3 = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.z = lagerspeed;
                else dirVector3.z = speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.z = -lagerspeed;
                else dirVector3.z = -speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = -lagerspeed;
                else dirVector3.x = -speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.x = lagerspeed;
                else dirVector3.x = speed;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = -lagerspeed;
                else dirVector3.y = -speed;
            }
            if (Input.GetKey(KeyCode.E))
            {
                if (Input.GetKey(KeyCode.LeftShift)) dirVector3.y = lagerspeed;
                else dirVector3.y = speed;
            }
            transform.Translate(dirVector3 * Time.deltaTime, Space.Self);
            transform.position = Vector3.ClampMagnitude(transform.position, 5);
        }

        /// <summary>
        /// 鼠标右键旋转摄像头
        /// </summary>
        private void RotationCamera()
        {
            if (Input.GetMouseButton(1))
            {
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                if (rotationY == 0)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, transform.localEulerAngles.z);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(-rotationY, rotationX, transform.localEulerAngles.z);
                }
            }
        }


        private void OnGUI()
        {
            //if (SceneManager.GetInstance().IsEntryScence)
            //{
            //    if (GUI.Button(new Rect(0, 0, 120, 60), "提交考试"))
            //    {
            //        Debug.Log("提交！");
            //        string Url = @"http://202.205.145.156:8017/open/api/v2/";
            //        string encodename = System.Web.HttpUtility.UrlEncode(Url);
            //        string showsend = Url + "data_upload?access_token=" + encodename;
            //        RestClient rs = new RestClient(Url);
            //        Stepspro st = new Stepspro();
            //        st.seq = 1;
            //        st.title = "实验步骤1";
            //        st.startTime = TimeHelp.GetTimeStampuse(DateTime.Now);
            //        st.endTime = TimeHelp.GetTimeStampuse(DateTime.Now);
            //        st.timeUsed = 123;
            //        st.expectTime = 2;
            //        st.maxScore = 10;
            //        st.score = 10;
            //        st.repeatCount = 1;
            //        st.evaluation = "优";
            //        st.scoringModel = "赋分模型";
            //        st.remarks = "备注";
            //        List<Stepspro> list1 = new List<Stepspro>();
            //        list1.Add(st);
            //        ParaObject p1 = new ParaObject();
            //        p1.username = "test";
            //        p1.title = "实验名称";
            //        p1.status = 1;
            //        p1.score = 100;
            //        p1.startTime = TimeHelp.GetTimeStampuse(DateTime.Now);
            //        p1.endTime = TimeHelp.GetTimeStampuse(DateTime.Now);
            //        p1.timeUsed = 123;
            //        p1.appid = "100400";
            //        p1.originId = "1";
            //        p1.steps = list1;
            //        string data = JsonConvert.SerializeObject((object)p1);
            //        string m = @"http://202.205.145.156:8017/open/api/v2/token?ticket=&appid=100400&signature=11CEF42C7B28BC1E04849EF26FEC37B0";
            //        string response = rs.Post(data, m);
            //        Debug.Log(response);
            //    }


            //    GUIStyle style = new GUIStyle();
            //    style.fontSize = 20;

            //    if (GUI.Button(new Rect(120, 0, 120, 60), "使用设备"))
            //    {
            //        List<Item> allItems = SceneManager.GetInstance().GetUsingItems();

            //        foreach (var item in allItems)
            //        {
            //            BoxCollider boxCollider;

            //            if (item.TryGetComponent<BoxCollider>(out boxCollider))
            //            {
            //                boxCollider.enabled = false;
            //            }
            //            MeshCollider meshCollider;
            //            if (item.TryGetComponent<MeshCollider>(out meshCollider))
            //            {
            //                meshCollider.enabled = false;
            //            }
            //        }
            //        SceneManager.GetInstance().SetMouseState(false, true);
            //    }

            //    if (GUI.Button(new Rect(240, 0, 120, 60), "移动设备"))
            //    {
            //        List<Item> allItems = SceneManager.GetInstance().GetUsingItems();
            //        foreach (var item in allItems)
            //        {
            //            BoxCollider boxCollider;
            //            if (item.TryGetComponent<BoxCollider>(out boxCollider))
            //            {
            //                boxCollider.enabled = true;
            //            }
            //            MeshCollider meshCollider;
            //            if (item.TryGetComponent<MeshCollider>(out meshCollider))
            //            {
            //                meshCollider.enabled = true;
            //            }
            //        }
            //        SceneManager.GetInstance().SetMouseState(false, false);
            //    }


            //    GUI.Label(new Rect(50, Screen.height / 2, Screen.width / 3, Screen.height), "操作提示:\n\n W:向前 \n\n S:向后 \n\n A:向左 \n\n D:向右 \n\n Q:向下 \n\n E:向上 \n\n R:旋转设备仪器 \n\n 鼠标左键:拖拽移动 \n\n 鼠标右键:按住旋转 ", style);


            //}
        }



    }
}
