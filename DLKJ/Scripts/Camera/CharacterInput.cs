using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    private CharacterController character;
    public Camera rotateCamera;
    private Vector3 dirVector3;
    public float speed = 1f;
    public float lagerspeed = 3f;
    public float sensitivityX = 2F;
    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        Rotate();
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
        Move(transform.rotation * dirVector3 * Time.deltaTime);
    }

    private void Move(Vector3 dir)
    {
        character.Move(dir);
    }
    private void FixedUpdate()
    {

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
