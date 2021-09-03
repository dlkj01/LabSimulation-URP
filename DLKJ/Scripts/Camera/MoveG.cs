using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveG : MonoBehaviour
{

    public enum TYPE
    {
        SELF,//�����ƶ�
        ENTIRETY//���屻�ƶ�
    }

    private float speed = 5f;//��ת�����ٶ�
    private float offsetX;//��תƫ����
    private float TotalOffset = 0f;
    private float OffsetX
    {
        get
        {
            return offsetX;
        }
        set
        {
            offsetX = value;
            TotalOffset += offsetX;
        }
    }
    public TYPE type = TYPE.SELF;
    public bool IsTrigger = false;


    private void OnTriggerEnter(Collider other)
    {
        IsTrigger = true;
        if (other.transform.name == "3CMLine ShortLoad")
        {
            transform.position = new Vector3(other.transform.position.x - 0.001f, other.transform.position.y + 0.008f, other.transform.position.z - 0.05f);
        }
    }
    public float distance = 11f;

    void Update()
    {
        ////�������������ʱ�򴴽�һ������ �ж��Ƿ�
        //if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
        //{

        //    Ray m_ray;
        //    RaycastHit m_hit;
        //    m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(m_ray, out m_hit))
        //    {
        //        Debug.DrawLine(m_ray.origin, m_hit.point);
        //        if (m_hit.transform.name == transform.name && type == TYPE.SELF)
        //        {
        //            OffsetX = Input.GetAxis("Mouse X");//��ȡ���x���ƫ����
        //            transform.Rotate(new Vector3(0, -OffsetX, 0) * speed, Space.World);//��ת����
        //        }

        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    IsTrigger = false;
        //}

        //if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)&& !IsTrigger)
        if (Input.GetMouseButton(0))
        {
            Ray m_ray;
            RaycastHit m_hit;
            m_ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(m_ray, out m_hit))
            {
                if (m_hit.transform.name == transform.name && type == TYPE.SELF)
                {
                    OffsetX = Input.GetAxis("Mouse X");//��ȡ���x���ƫ����
                    transform.Rotate(new Vector3(0, -OffsetX, 0) * speed, Space.World);//��ת����
                }
                if (m_hit.transform.name == transform.name && type == TYPE.ENTIRETY)
                {
                    //��ȡ��Ҫ�ƶ����������ת��Ļ����
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(distance,m_hit.transform.position.y, m_hit.transform.position.z));
                    //��ȡ���λ��
                    Vector3 mousePos = Input.mousePosition;
                    //��Ϊ���ֻ��X��Y�ᣬ����Ҫ��������Z��
                    mousePos.z = screenPos.z;
                    //��������Ļ����ת������������
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    //���������ƶ�
                    transform.position = worldPos;
                    if (transform.position.y <= 0.95f)
                    {
                        transform.position = new Vector3(transform.position.x, 0.95f, transform.position.z);
                    }
                }
            }



        }


    }


}
