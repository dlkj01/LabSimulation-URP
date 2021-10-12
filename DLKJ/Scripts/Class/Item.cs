using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DLKJ
{
    public class Item : MonoBehaviour
    {
        public int ID = -1;
        public string itemName = "";
        public Texture2D icon;
        public RenderTexture renderTexture;
        public bool moveable = true;
        public LibraryType libraryType = LibraryType.None;


        [SerializeField] public List<Link> ports = new List<Link>();
        public List<Condition> linkConditions = new List<Condition>();
        public Link linkPort = null;

        [Header("Default Position and Rotation")]
        public Vector3 portDefaultPosition = Vector3.zero;
        public Vector3 portDefaultEuler = Vector3.zero;


        [Header("Drag And Rotate Setting")]
        private bool dragAble = false;
        private bool startDetection = false;
        private Coroutine magicCoroutine = null;

        private Vector3 dist;
        private float mouseDownX;
        private float mouseDownY;

        private BoxCollider boxCollider;

        [HideInInspector] private List<string> LinkNames;//连接着的设备
        private void Awake()
        {
            boxCollider = transform.GetComponent<BoxCollider>();

            for (int i = 0; i < ports.Count; i++)
            {
                ports[i].ParentItem = this;
            }
        }

        private void OnEnable()
        {
            EventManager.OnDetectionEvent += UpdateTriggerState;
        }

        private void OnDisable()
        {
            EventManager.OnDetectionEvent -= UpdateTriggerState;
        }

        void SetDragable(bool state)
        {
            dragAble = state;
            if (boxCollider)
            {
                boxCollider.enabled = state;
            }
        }

        void OnAttach(TargetPort target)
        {
            SetDragable(false);
            transform.position = new Vector3(target.targetPosition.x, target.targetPosition.y, transform.position.z);

            if (magicCoroutine != null) StopCoroutine(magicCoroutine);
            magicCoroutine = StartCoroutine(Move(target));
            // magicCoroutine = StartCoroutine(MoveToTarget(target));
        }

        IEnumerator Move(TargetPort target)
        {
            BroadcastMessage("StopDetection", SendMessageOptions.RequireReceiver);
            double reDistance = Vector3.Distance(target.selfPort.position, target.targetPort.transform.position);

            while (reDistance > target.distance)// 0.005243f
            {
                reDistance = Vector3.Distance(target.selfPort.position, target.targetPort.transform.position);
                //transform.position = Vector3.Lerp(transform.position, target.targetPosition, Time.deltaTime * target.speed);
                Vector3 moveVector = target.targetPort.transform.position - target.selfPort.position;
                transform.Translate(moveVector * 1 * Time.deltaTime, Space.World);
                yield return new WaitForFixedUpdate();
            }

            SetDragable(true);
            if (CorrectLink(target.targetPort))  //检查连接的是否是正确目标的端口
            {
                linkPort = target.targetPort;
            }

            if (target.linkNextOne)
            {
                EventManager.OnLinkNext();
            }
        }

        public bool CorrectLink(Link targetPort)
        {
            Condition correctCondition = null;
            for (int i = 0; i < linkConditions.Count; i++)
            {
                if (linkConditions[i].data.correct)
                {
                    correctCondition = linkConditions[i];
                    break;
                }
            }
            if (correctCondition == null) return false;

            if (correctCondition.data.itemID == targetPort.ParentItem.ID)
            {
                if (correctCondition.data.portsID == targetPort.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public void AutoConnect()
        {
            Vector3 targetPosition;
            for (int i = 0; i < linkConditions.Count; i++)
            {
                if (!linkConditions[i].data.correct) continue;
                Item targetItem = SceneManager.GetInstance().GetLabItemByID(linkConditions[i].data.itemID);
                Link _targetPort = targetItem.GetPortByPortsID(linkConditions[i].data.portsID);
                targetPosition = _targetPort.transform.position - ports[0].transform.localPosition;

                TargetPort targetPort = new TargetPort();
                targetPort.targetPosition = targetPosition;
                targetPort.targetPort = _targetPort;
                targetPort.selfPort = ports[0].transform;
                targetPort.distance = _targetPort.portCollider.bounds.size.z;
                targetPort.speed = _targetPort.moveSpeed;
                targetPort.linkNextOne = true;
                OnAttach(targetPort);
            }
        }

        private Vector2 dir;
        public void OnMouseDown()
        {
            dragAble = true;
            startDetection = false;
            SceneManager.GetInstance().SetMouseState(true);
            BroadcastMessage("StopDetection", SendMessageOptions.DontRequireReceiver);
            for (int i = 0; i < linkConditions.Count; i++)
            {
                EventManager.OnDetection(linkConditions[i].data.itemID, linkConditions[i].data.portsID, true);
            }

            dist = Camera.main.WorldToScreenPoint(transform.position);
            dir = new Vector2(Input.mousePosition.x - dist.x, Input.mousePosition.y - dist.y);
        }

        public void OnMouseUp()
        {
            //rigidbody.isKinematic = false;
            Destroy(transform.gameObject.GetComponent<Rigidbody>());
            dragAble = false;
            dragAble = true;
            startDetection = false;
            SceneManager.GetInstance().SetMouseState(false);
            BroadcastMessage("StopDetection", SendMessageOptions.DontRequireReceiver);
            TriggerDetection(true);
        }

        private float eulers = 0;
        public void OnMouseDrag()
        {
            if (dragAble && moveable)
            {
                if (!startDetection)
                {
                    startDetection = true;
                    BroadcastMessage("InitiateDetection", ports[0].ID, SendMessageOptions.DontRequireReceiver);
                    TriggerDetection(false);
                }
                //transform.position = MyScreenToworld(Input.mousePosition, transform);
                transform.position = ScreenToWorld();

                if (Input.GetKeyDown(KeyCode.R))
                {
                    eulers += 90;
                    transform.eulerAngles = new Vector3(0, eulers, 0);
                    //transform.Rotate(new Vector3(0, eulers, 0), Space.World);
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        public void TriggerDetection(bool state)
        {
            for (int i = 0; i < linkConditions.Count; i++)
            {
                EventManager.OnDetection(linkConditions[i].data.itemID, linkConditions[i].data.portsID, state);
            }
        }

        Vector3 MyScreenToworld(Vector3 mousepos, Transform targetTransform)
        {
            Vector3 dir = -targetTransform.position + Camera.main.transform.position;
            Vector3 normardir = Vector3.Project(Camera.main.transform.forward, dir);
            //计算节点，需要知道处置屏幕的投影距离
            Vector3 worldpos = Camera.main.ScreenToWorldPoint(new Vector3(mousepos.x, mousepos.y, normardir.magnitude));//normardir.magnitude
            return worldpos;
        }

        Vector3 ScreenToWorld()
        {
            Vector3 curPos = new Vector3(Input.mousePosition.x - dir.x, Input.mousePosition.y - dir.y, dist.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
            worldPos = new Vector3(Mathf.Clamp(worldPos.x, SceneManager.GetInstance().min.x, SceneManager.GetInstance().max.x),
                                    Mathf.Clamp(worldPos.y, SceneManager.GetInstance().min.y, SceneManager.GetInstance().max.y),
                                    Mathf.Clamp(worldPos.z, SceneManager.GetInstance().min.z, SceneManager.GetInstance().max.z));
            return worldPos;
        }

        private Transform mMark;

        private Transform tMark;

        //public Transform TAssambleObject;
        private IEnumerator MoveToTarget(TargetPort targetPort)
        {
            tMark = targetPort.selfPort.transform;
            mMark = targetPort.targetPort.transform;
            tMark.parent.transform.localRotation = Quaternion.identity;
            if (mMark.localRotation.z == -tMark.localRotation.z)
            {
                Debug.Log("反向");
                mMark.parent.Rotate(mMark.parent.transform.up, 180, Space.World);
            }
            Vector3 RotateAix = Vector3.Cross(mMark.transform.forward, tMark.transform.forward);
            float angle = Vector3.Angle(mMark.transform.forward, tMark.transform.forward);
            mMark.transform.parent.Rotate(RotateAix, angle, Space.World);
            bool start = true;
            while (start)
            {
                //第一步
                float Angle = Vector3.Angle(tMark.transform.up, mMark.transform.up);
                tMark.transform.parent.Rotate(tMark.transform.forward, Angle * 10f * Time.deltaTime, Space.World);
                //第二步
                Vector3 moveVector = -tMark.transform.position + mMark.transform.position;
                tMark.transform.parent.transform.Translate(moveVector * 1 * Time.deltaTime, Space.World);
                if (Angle == 0 && moveVector == Vector3.zero)
                {
                    Debug.Log("停止");
                    break;
                }
                if (Vector3.Distance(mMark.transform.parent.position, tMark.transform.parent.position) >= tMark.transform.parent.localScale.z && Vector3.Distance(mMark.transform.position, tMark.transform.position) < 0.005f)
                {
                    start = false;
                    Debug.Log("停止");
                }
                yield return new WaitForSeconds(0.001f);
            }
        }
        public void UpdateTriggerState(int itemID, int portID, bool isTrigger)
        {
            if (itemID != ID) return;
            for (int i = 0; i < ports.Count; i++)
            {
                if (portID == ports[i].ID)
                {
                    ports[i].SetTriggerState(isTrigger);
                }
            }
        }

        public void SetMoveable(bool enableMove)
        {
            moveable = enableMove;
        }

        public String[] GetLinkPorts()
        {
            String[] names = new String[ports.Count];

            for (int i = 0; i < ports.Count; i++)
            {
                names[i] = ports[i].gameObject.name;
            }
            return names;
        }

        public int GetPortsIndex(int portID)
        {
            for (int i = 0; i < ports.Count; i++)
            {
                if (ports[i].ID == portID)
                {
                    return i;
                }
            }
            return ports[0].ID;
        }

        public Link GetPortByIndex(int index)
        {
            for (int i = 0; i < ports.Count; i++)
            {
                if (i == index)
                {
                    return ports[i];
                }
            }
            return ports[0];
        }

        public Link GetPortByPortsID(int ID)
        {
            for (int i = 0; i < ports.Count; i++)
            {
                if (ports[i].ID == ID)
                {
                    return ports[i];
                }
            }
            return ports[0];
        }
    }
}