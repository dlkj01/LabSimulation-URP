using System;
using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

namespace DLKJ
{
    public enum DirectionType
    {
        Horizontal,     //横向
        Vertical,       //纵向
    }

    public class Item : MonoBehaviour
    {
        public int ID = -1;
        public string itemName = "";
        public Texture2D icon;
        public RenderTexture renderTexture;
        public bool moveable = true;
        public LibraryType libraryType = LibraryType.None;
        public DirectionType directionType = DirectionType.Horizontal;


        [SerializeField] public List<Link> ports = new List<Link>();
        public List<Condition> linkConditions = new List<Condition>();
        public Link linkPort = null;

        [Header("Default Position and Rotation")]
        public Vector3 portDefaultPosition = Vector3.zero;
        public Vector3 portDefaultEuler = Vector3.zero;


        [Header("Drag And Rotate Setting")]
        private bool dragAble = false;
        private bool usingItem = false;
        private bool startDetection = false;
        private Coroutine magicCoroutine = null;

        private Vector3 dist;
        private float mouseDownX;
        private float mouseDownY;
        private BoxCollider boxCollider;
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

        public void OnUsingItem(bool isUsing)
        {
            usingItem = isUsing;
            if (isUsing)
            {
                boxCollider.enabled = false;
            }
            else
            {
                if (dragAble)
                {
                    boxCollider.enabled = true;
                }
            }
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
            switch (directionType)
            {
                case DirectionType.Horizontal:
                    {
                        transform.position = new Vector3(target.targetPosition.x, target.targetPosition.y, transform.position.z);
                    }
                    break;
                case DirectionType.Vertical:
                    {
                        transform.position = new Vector3(transform.position.x - 0.25f, target.targetPort.transform.position.y, target.targetPort.transform.position.z);
                    }
                    break;
                default:
                    break;
            }

            if (magicCoroutine != null) StopCoroutine(magicCoroutine);
            magicCoroutine = StartCoroutine(Move(target));
        }

        IEnumerator Move(TargetPort target)
        {
            BroadcastMessage("StopDetection", SendMessageOptions.RequireReceiver);
            double reDistance = Vector3.Distance(target.selfPort.transform.position, target.targetPort.transform.position);

            while (reDistance > target.distance)// 0.005243f
            {
                reDistance = Vector3.Distance(target.selfPort.transform.position, target.targetPort.transform.position);
                //transform.position = Vector3.Lerp(transform.position, target.targetPosition, Time.deltaTime * target.speed);
                Vector3 moveVector = target.targetPort.transform.position - target.selfPort.transform.position;
                transform.Translate(moveVector * 1 * Time.deltaTime, Space.World);
                yield return new WaitForFixedUpdate();
            }

            target.targetPort.LinkedItem = this;
            SetDragable(true);
            if (CorrectLink(target.targetPort))  //检查连接的是否是正确目标的端口
            {
                linkPort = target.targetPort;
            }

            if (target.linkNextOne)
            {

                EventManager.OnLinkNext(SceneManager.GetInstance().currentLab.currentStep.keyItems);
            }
        }

        public bool CorrectLink(Link targetPort)
        {
            Step currentStep = SceneManager.GetInstance().currentLab.currentStep;
            List<Item> correctLinkItems = currentStep.keyItems;
            bool contains = false;
            for (int i = 0; i < correctLinkItems.Count; i++)
            {
                if (correctLinkItems[i].ID == this.ID)
                {
                    contains = true;
                    break;
                }
            }
            if (contains)
            {
                Item previousItem = currentStep.GetPreviousLinkItem(this);
                if (previousItem)
                {
                    if (previousItem.ContainsPort(targetPort))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        bool ContainsPort(Link port) {
            bool contains = false;
            for (int i = 0; i < ports.Count; i++)
            {
                if (ports[i].ID == port.ID)
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        public void AutoConnect(Condition targetCondition = null)
        {
            Vector3 targetPosition;
            for (int i = 0; i < linkConditions.Count; i++)
            {

                if (!linkConditions[i].data.correct) continue;
                Item targetItem = SceneManager.GetInstance().GetLabItemByID(linkConditions[i].data.itemID);
                if (targetItem == null) continue;

                Link _targetPort;
                if (targetCondition != null)
                {
                    if (targetCondition != linkConditions[i]) continue;
                    _targetPort = targetItem.GetPortByPortsID(targetCondition.data.portsID);
                }
                else
                {
                    _targetPort = targetItem.GetPortByPortsID(linkConditions[i].data.portsID);
                }

                if (_targetPort.LinkedItem != null)
                {
                    _targetPort.LinkedItem.Revert();
                }

                if (libraryType == LibraryType.Wires)
                {
                    for (int a = 0; a < ports.Count; a++)
                    {
                        if (ports[a].dragAble)
                        {
                            ports[a].transform.SetParent(_targetPort.transform);
                            if (targetItem.itemName == "晶体检波器")
                            {
                                ports[a].transform.localPosition = targetItem.portDefaultPosition;
                            }
                            else
                            {
                                ports[a].transform.localPosition = Vector3.zero + ports[a].offset;
                            }

                            linkPort = _targetPort;
                            targetItem.linkPort = ports[a];
                            ports[a].transform.localRotation = Quaternion.Euler(targetItem.portDefaultEuler);
                            ports[a].dragAble = false;
                            break;
                        }
                    }
                    dragAble = false;
                    EventManager.OnLinkNext(SceneManager.GetInstance().currentLab.currentStep.keyItems);
                }
                else
                {
                    targetPosition = _targetPort.transform.position - ports[0].transform.localPosition;
                    TargetPort targetPort = new TargetPort();
                    targetPort.targetPosition = targetPosition;
                    targetPort.targetPort = _targetPort;
                    targetPort.selfPort = ports[0];
                    if (itemName == "晶体检波器")
                    {
                        if (SceneManager.GetInstance().currentLab.currentStepIndex == 2)
                        {
                            RotationY();
                            targetPort.distance = _targetPort.portCollider.bounds.size.x;
                        }
                        else
                        {
                            targetPort.distance = _targetPort.portCollider.bounds.size.z;
                        }
                    }
                    else
                    {
                        targetPort.distance = _targetPort.portCollider.bounds.size.z;
                    }

                    targetPort.speed = _targetPort.moveSpeed;
                    targetPort.linkNextOne = true;
                    OnAttach(targetPort);
                }
            }
        }

        void Revert()
        {
            linkPort = null;
            ItemDB itemDB = DBManager.GetInstance().GetDB<ItemDB>();
            transform.position = itemDB.GetItemByID(ID).transform.position;
            if (ports.Count > 1)
            {
                if (ports[1].LinkedItem)
                {
                    ports[1].LinkedItem.Revert();
                }
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
            Destroy(transform.gameObject.GetComponent<Rigidbody>());
            dragAble = true;
            startDetection = false;
            SceneManager.GetInstance().SetMouseState(false);
            BroadcastMessage("StopDetection", SendMessageOptions.DontRequireReceiver);
            TriggerDetection(true);
        }

        private float eulers = 0;
        public void OnMouseDrag()
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
                return;
            if (dragAble && moveable&& !usingItem)
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
                    RotationY();
                }

                if (Input.GetKeyDown(KeyCode.T))
                {
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        public void RotationY()
        {
            eulers = transform.eulerAngles.y;
            eulers += 90;

            transform.eulerAngles = new Vector3(0, eulers, 0);
            switch (directionType)
            {
                case DirectionType.Horizontal:
                    {
                        directionType = DirectionType.Vertical;
                    }
                    break;
                case DirectionType.Vertical:
                    {
                        directionType = DirectionType.Horizontal;
                    }
                    break;
                default:
                    break;
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