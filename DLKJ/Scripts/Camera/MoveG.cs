using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveG : MonoBehaviour
{

    public enum TYPE
    {
        SELF,//自身被移动
        ENTIRETY//整体被移动
    }

    private float speed = 5f;//旋转跟随速度
    private float offsetX;//旋转偏移量
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
        ////当点击鼠标左键的时候创建一条射线 判断是否
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
        //            OffsetX = Input.GetAxis("Mouse X");//获取鼠标x轴的偏移量
        //            transform.Rotate(new Vector3(0, -OffsetX, 0) * speed, Space.World);//旋转物体
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
                    OffsetX = Input.GetAxis("Mouse X");//获取鼠标x轴的偏移量
                    transform.Rotate(new Vector3(0, -OffsetX, 0) * speed, Space.World);//旋转物体
                }
                if (m_hit.transform.name == transform.name && type == TYPE.ENTIRETY)
                {
                    //获取需要移动物体的世界转屏幕坐标
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(distance,m_hit.transform.position.y, m_hit.transform.position.z));
                    //获取鼠标位置
                    Vector3 mousePos = Input.mousePosition;
                    //因为鼠标只有X，Y轴，所以要赋予给鼠标Z轴
                    mousePos.z = screenPos.z;
                    //把鼠标的屏幕坐标转换成世界坐标
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    //控制物体移动
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
