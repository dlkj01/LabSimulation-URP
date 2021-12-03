using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DLKJ
{

    public class Link : MonoBehaviour
    {
        public bool dragAble = false;
        public int ID = -1;
        private float scope = 0.04f /*0.02f*/; //radius
        public float moveSpeed = 1f;
        public MeshCollider portCollider;
        public Vector3 offset;
        [HideInInspector] public MeshRenderer meshRenderer;

        public Rigidbody blidingObject;

        public Item parent;
        private Item linkedItem = null;
        private bool detecting = false;
        private Coroutine decectCoroutine = null;
        private Coroutine wireCoroutine = null;

        private Vector3 dist;
        private float mouseDownX;
        private float mouseDownY;
        [HideInInspector] public string ItemName;
        private void Awake()
        {
            ItemName = GetComponentInParent<Item>().itemName;
            if (portCollider == null) portCollider = transform.GetComponent<MeshCollider>();
            if (meshRenderer == null) meshRenderer = transform.GetComponent<MeshRenderer>();
            if (transform.name == "weibo_kou" || transform.name == "pinxuan_kou")
            {
                portCollider.enabled = false;
            }
        }

        public Item ParentItem { get { return parent; } set { parent = value; } }
        public Item LinkedItem { get { return linkedItem; } set { linkedItem = value; } }

        public void OnDrawGizmosSelected()
        {
            if (detecting)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position, scope);
            }
        }

        public void SetTriggerState(bool state)
        {
            portCollider.isTrigger = state;
            if (state) meshRenderer.material.color = Color.white;
            else meshRenderer.material.color = Color.green;
        }

        public void InitiateDetection(int portsID)
        {
            if (portsID != ID) return;
            detecting = true;
            meshRenderer.material.color = Color.green;
            if (decectCoroutine != null) StopCoroutine(decectCoroutine);
            decectCoroutine = StartCoroutine(Detecting());
        }

        public void StopDetection()
        {
            detecting = false;
            meshRenderer.material.color = Color.white;
            if (decectCoroutine != null) StopCoroutine(decectCoroutine);
        }
        float triggerDis;
        IEnumerator Detecting()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            while (detecting)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, scope, 1 << LayerMask.NameToLayer("Port"));
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].isTrigger == false)
                    {
                        //Debug.Log("物体名称:" + colliders[i].gameObject.name + "  世界坐标:" + colliders[i].transform.TransformPoint(colliders[i].transform.localPosition).ToString("f6"));
                        //EventManager.OnAttractItem(colliders[i].transform.TransformPoint(colliders[i].transform.position));
                        //SendMessageUpwards("OnAttach", colliders[i].transform.parent.GetComponent<Item>().NewPort().transform.position, SendMessageOptions.RequireReceiver);

                        //float distance = Mathf.Abs(targetPosition.z) - portCollider.bounds.size.z;//0.005243f;碰撞体和网格大小打印均为零；
                        //targetPosition = new Vector3(targetPosition.x, targetPosition.y, distance);

                        //***注意当打印的向量值比较小的时候打印值和实际值是有误差的，不能参考打印的数值,Debug的是小数点后几位的值***//
                        //根据子物体的世界坐标算父物体的移动目标点目前计算不算准确，后期有精确的算法可以替换//

                        Link target = colliders[i].transform.GetComponent<Link>();
                        Debug.Log("连接的目标："+target.ParentItem.itemName);
                        if (SceneManager.GetInstance().currentLab.currentStep.Contains(ParentItem.ID))
                        {
                            StartCoroutine(stay(target, moveSpeed));
                        }
                        else
                        {
                            EventManager.OnTips(TipsType.Toast, "当前步骤不允许连接该器件！");
                            ParentItem.Revert();
                        }
                    }
                }
                yield return waitForFixedUpdate;
            }
        }

        public void AutoConnect(Link target, float speed)
        {
            Debug.Log(parent.itemName+"的连接对象：" + target.ParentItem.itemName);
            Vector3 targetPosition = target.transform.position - transform.localPosition;
            TargetPort targetPort = new TargetPort();
            targetPort.targetPosition = targetPosition;
            targetPort.targetPort = target;
            targetPort.selfPort = this;
            targetPort.selfParent = parent;
            switch (parent.directionType)
            {
                case DirectionType.Horizontal:
                    targetPort.distance = portCollider.bounds.size.z;
                    break;
                case DirectionType.Vertical:
                    targetPort.distance = portCollider.bounds.size.x;
                    break;
                default:
                    break;
            }
            targetPort.speed = speed;
            targetPort.linkNextOne = false;
            //此消息只有父物体能接收
            SendMessageUpwards("OnAttach", targetPort, SendMessageOptions.RequireReceiver);
            //  StartCoroutine(RayTest(target));
        }


        IEnumerator stay(Link target, float speed)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
            yield return waitForSeconds;
            if (scope >= Vector3.Distance(transform.position, target.transform.position))
                AutoConnect(target, speed);
        }



        IEnumerator RayTest(Link target)
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

            float distance = Vector3.Distance(transform.position, target.transform.position);
            float Z = target.portCollider.bounds.size.z;
            while (distance > Z)
            {
                distance = Vector3.Distance(transform.position, target.transform.position);
                yield return waitForFixedUpdate;
            }
            SendMessageUpwards("TriggerEnter", true, SendMessageOptions.RequireReceiver);
        }

        public void OnMouseDown()
        {
            if (dragAble)
            {
                detecting = true;
                isDraging = true;
                dist = Camera.main.WorldToScreenPoint(transform.position);
                mouseDownX = Input.mousePosition.x - dist.x;
                mouseDownY = Input.mousePosition.y - dist.y;
            }
        }

        bool isDraging = false;
        public void OnMouseUp()
        {
            if (dragAble)
            {
                detecting = false;
                isDraging = false;
                if (wireCoroutine != null) StopCoroutine(wireCoroutine);
                wireCoroutine = null;
                ParentItem.TriggerDetection(true);
            }
        }

        public void OnMouseDrag()
        {
            if (dragAble && isDraging)
            {
                transform.position = ScreenToWorld();
                isDraging = true;
                if (wireCoroutine == null)
                {
                    ParentItem.TriggerDetection(false);
                    wireCoroutine = StartCoroutine(WireDetecting());
                }
            }
        }

        IEnumerator WireDetecting()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            while (isDraging)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, scope, 1 << LayerMask.NameToLayer("Port"));
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].isTrigger == false)
                    {
                        Link target = colliders[i].transform.GetComponent<Link>();
                        if (ParentItem.CorrectLink(target, true))
                        {
                            transform.SetParent(target.transform);
                            if (target.parent.itemName == "晶体检波器")
                                transform.localPosition = target.parent.portDefaultPosition;
                            else
                                transform.localPosition = Vector3.zero + offset;
                            transform.localRotation = Quaternion.Euler(target.parent.portDefaultEuler);
                            isDraging = false;
                            ParentItem.linkPort = target;
                            target.ParentItem.linkPort = this;
                        }
                    }
                }
                yield return waitForFixedUpdate;
            }
        }

        Vector3 ScreenToWorld()
        {
            Vector3 curPos = new Vector3(Input.mousePosition.x - mouseDownX, Input.mousePosition.y - mouseDownX, dist.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
            return worldPos;
        }
    }

    public struct TargetPort
    {
        public Vector3 targetPosition;
        public Link targetPort;
        public Link selfPort;
        public Item selfParent;
        public float distance;
        public float speed;
        public bool linkNextOne;
    }
}