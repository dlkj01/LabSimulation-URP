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

        private Item parent;
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
            if (transform.name== "weibo_kou"|| transform.name == "pinxuan_kou")
            {
                portCollider.enabled = false;
            }
        }

        public Item ParentItem { get { return parent; } set { parent = value; } }

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
                        //Debug.Log("��������:" + colliders[i].gameObject.name + "  ��������:" + colliders[i].transform.TransformPoint(colliders[i].transform.localPosition).ToString("f6"));
                        //EventManager.OnAttractItem(colliders[i].transform.TransformPoint(colliders[i].transform.position));
                        //SendMessageUpwards("OnAttach", colliders[i].transform.parent.GetComponent<Item>().NewPort().transform.position, SendMessageOptions.RequireReceiver);

                        //float distance = Mathf.Abs(targetPosition.z) - portCollider.bounds.size.z;//0.005243f;��ײ��������С��ӡ��Ϊ�㣻
                        //targetPosition = new Vector3(targetPosition.x, targetPosition.y, distance);

                        //***ע�⵱��ӡ������ֵ�Ƚ�С��ʱ���ӡֵ��ʵ��ֵ�������ģ����ܲο���ӡ����ֵ***//
                        //��������������������㸸������ƶ�Ŀ���Ŀǰ���㲻��׼ȷ�������о�ȷ���㷨�����滻//

                        Link target = colliders[i].transform.GetComponent<Link>();
                        //AutoConnect(target, moveSpeed);


                        StartCoroutine(stay(target, moveSpeed));
                        //Vector3 targetPosition = colliders[i].transform.position - transform.localPosition;

                        //TargetPort targetPort = new TargetPort();
                        //targetPort.targetPosition = targetPosition;
                        //targetPort.targetPort = colliders[i].transform.GetComponent<Link>();
                        //targetPort.selfPort = transform;
                        //targetPort.distance = portCollider.bounds.size.z;
                        //targetPort.speed = moveSpeed;

                        ////����Ϣֻ�и������ܽ���
                        //SendMessageUpwards("OnAttach", targetPort, SendMessageOptions.RequireReceiver);
                    }
                }
                yield return waitForFixedUpdate;
            }
        }

        public void AutoConnect(Link target, float speed)
        {
            Vector3 targetPosition = target.transform.position - transform.localPosition;
            TargetPort targetPort = new TargetPort();
            targetPort.targetPosition = targetPosition;
            targetPort.targetPort = target;
            targetPort.selfPort = transform;
            targetPort.distance = portCollider.bounds.size.z;
            targetPort.speed = speed;
            targetPort.linkNextOne = false;
            //����Ϣֻ�и������ܽ���
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
                        if (ParentItem.CorrectLink(target))
                        {
                            transform.SetParent(target.transform);
                            if (target.parent.itemName == "����첨��")
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
        public Transform selfPort;
        public float distance;
        public float speed;
        public bool linkNextOne;
    }
}