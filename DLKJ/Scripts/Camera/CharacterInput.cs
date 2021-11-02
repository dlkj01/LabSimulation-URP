using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    private CharacterController character;
    public Camera rotateCamera;
    private float defaultSpeed;
    public float speed = 1f;
    public float speedRatio = 3f;//ËÙ¶È±¶ÂÊ
    public float sensitivityX = 2F;
    private void Awake()
    {
        character = GetComponent<CharacterController>();
        defaultSpeed = speed;
    }
    Vector3 xDir;
    Vector3 yDir;
    Vector3 zDir;
    float x;
    float y;
    void Update()
    {
        Rotate();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = speedRatio * speed;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = defaultSpeed;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        xDir = transform.right * x;
        zDir = rotateCamera.transform.forward * y;
        yDir = Vector3.zero;
        if (Input.GetKey(KeyCode.E))
            yDir = Vector3.up;
        if (Input.GetKey(KeyCode.Q))
            yDir = Vector3.down;
        Move((xDir + zDir + yDir) * speed * Time.deltaTime);
    }

    private void Move(Vector3 dir)
    {
        character.Move(dir);
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationX, transform.localEulerAngles.z);
        }
    }
}
