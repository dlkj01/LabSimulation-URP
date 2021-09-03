using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float pitch;
    public float maxViewDistance = 22f;
    public float minViewDistance = 5f;
    public float targetFieldOfView = 30;

    [HeaderAttribute("Pan Settings")]
    public float panSpeed = 15.0f;
    public float mousePanMultiplier = 1.0f;
    public float mouseDeadZone = 100.0f;
    public float terrainPadding = 10.0f;


    //Zoom Settings
    [HeaderAttribute("Zoom Settings")]
    public float zoomSpeed = 1.0f;
    public float mouseZoomMultiplier = 5.0f;
    public float zoomMin = 2.0f;
    public float zoomMax = 50.0f;


    //Rotation Settings
    [HeaderAttribute("Rotation Settings")]
    public float rotationSpeed = 50.0f;
    public float mouseRotationMultiplier = 0.2f;
    public float rotationMax = 65.0f;
    public float rotationMin = 5.0f;


    //General Speed Settings
    [HeaderAttribute("General Speed Settings")]
    public bool smoothing = true;
    public float smoothingFactor = 0.1f;


    //Generic Settings
    [HeaderAttribute("Generic Settings")]
    public bool adaptToTerrainHeight = true;
    public float mouseEdgeBoundary = 40.0f;
    public static Vector3 cameraTarget;
    public Vector3 lastMousePos;
    public Vector3 mouseClicked;
    public bool useLimits = true;

    //Private Settings
    private Vector3 lastPanSpeed = Vector3.zero;
    private Vector3 cameraPosition;

    public LayerMask groundLayer;

    void Start()
    {
        cameraTarget = transform.position;
        lastMousePos = Vector3.zero;
        mouseClicked = Vector3.zero;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.cyan);

        Rotate();
        Pan();
        Zoom();
        UpdatePosition();

        lastMousePos = Input.mousePosition;
    }

    private void Pan()
    {
        Vector3 whereTo = Vector3.zero;

        //Move the camera with the keyboard
        if (KeyboardInput())
        {
            if (Input.GetKey(KeyCode.W))
                whereTo += Vector3.forward;

            if (Input.GetKey(KeyCode.A))
                whereTo += Vector3.left;

            if (Input.GetKey(KeyCode.S))
                whereTo += Vector3.back;

            if (Input.GetKey(KeyCode.D))
                whereTo += Vector3.right;
        }

        //Move the camera with the right mouse button (keep pressed and move around)
        if (MouseInput())
        {
            if (Input.GetMouseButton(1) && mouseClicked == Vector3.zero)
                mouseClicked = Input.mousePosition;

            if (Input.GetMouseButton(1))
            {
                if (mouseClicked != Vector3.zero)
                {
                    if ((mouseClicked.y < Input.mousePosition.y) && ((Input.mousePosition.y - mouseClicked.y) > mouseDeadZone))
                        whereTo += Vector3.forward * (Input.mousePosition.y - mouseClicked.y) / 150 * mousePanMultiplier;

                    if ((mouseClicked.x < Input.mousePosition.x) && ((Input.mousePosition.x - mouseClicked.x) > mouseDeadZone))
                        whereTo += Vector3.right * (Input.mousePosition.x - mouseClicked.x) / 150 * mousePanMultiplier;

                    if ((mouseClicked.y > Input.mousePosition.y) && ((mouseClicked.y - Input.mousePosition.y) > mouseDeadZone))
                        whereTo += Vector3.back * (mouseClicked.y - Input.mousePosition.y) / 150 * mousePanMultiplier;

                    if ((mouseClicked.x > Input.mousePosition.x) && ((mouseClicked.x - Input.mousePosition.x) > mouseDeadZone))
                        whereTo += Vector3.left * (mouseClicked.x - Input.mousePosition.x) / 150 * mousePanMultiplier;
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
            mouseClicked = Vector3.zero;

        //Move the camera by placing the mouse cursor on the edges of the screen
        if (MouseOverScreenEdge())
        {
            if (Input.mousePosition.y > (Screen.height - mouseEdgeBoundary))
                whereTo += Vector3.forward * mousePanMultiplier;

            if (Input.mousePosition.x > (Screen.width - mouseEdgeBoundary))
                whereTo += Vector3.right * mousePanMultiplier;

            if (Input.mousePosition.y < mouseEdgeBoundary)
                whereTo += Vector3.back * mousePanMultiplier;

            if (Input.mousePosition.x < mouseEdgeBoundary)
                whereTo += Vector3.left * mousePanMultiplier;
        }

        //Now that the destination is set we move the camera
        SmoothIt(whereTo);
    }

    private void Zoom()
    {
        Vector3 whereTo = Vector3.zero;
        if (KeyboardInput())
        {
            if (Input.GetKey(KeyCode.R))
                whereTo += Vector3.down * zoomSpeed;

            if (Input.GetKey(KeyCode.F))
                whereTo += Vector3.up * zoomSpeed;
        }

        if (MouseInput())
        {
            //if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            //    transform.position += Vector3.forward * mouseZoomMultiplier;
            //    Debug.Log("Scroll 01:"+ Vector3.forward * mouseZoomMultiplier);
            //}


            //if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            //    Debug.Log("Scroll 02");
            //    transform.position += Vector3.back * mouseZoomMultiplier;
            //}

            transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * mouseZoomMultiplier );
            Camera.main.fieldOfView = Mathf.SmoothStep(Camera.main.fieldOfView, targetFieldOfView, 1);
        }

        //if (useLimits)
        //{
        //    if ((transform.position.z >= zoomMin && whereTo.z < 0) || (transform.position.z <= zoomMax && whereTo.z > 0))
        //        SmoothIt(whereTo);
        //}
        //else
        //{
        //    SmoothIt(whereTo);
        //}
    }

    private void Rotate()
    {
        float yRotation = 0.0f;
        float xRotation = 0.0f;

        if (KeyboardInput())
        {
            if (Input.GetKey(KeyCode.Q))
                yRotation = -1.0f;

            if (Input.GetKey(KeyCode.E))
                yRotation = 1.0f;
        }
        //transform.Rotate(0, yRotation, 0);
        Quaternion targetRotation = new Quaternion(transform.rotation.x, transform.rotation.y + yRotation, transform.rotation.z, transform.rotation.w);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 1*Time.deltaTime);

        if (MouseInput())
        {
            if (Input.GetMouseButton(2))
            {
                Vector3 mousePosition = (Input.mousePosition - lastMousePos);
                if (Input.mousePosition.x != lastMousePos.x)
                {
                    yRotation += mousePosition.x * mouseRotationMultiplier;
                    //Rotate the camera horizontally on the Y axis
                    transform.Rotate(0, yRotation, 0);
                }
                if (Input.mousePosition.y != lastMousePos.y)
                {
                    xRotation -= mousePosition.y * mouseRotationMultiplier;

                    if (useLimits)
                    {
                        float desiredXRotation = transform.eulerAngles.x + xRotation;
                        if (desiredXRotation >= rotationMin && desiredXRotation <= rotationMax && useLimits)
                            transform.Rotate(xRotation, 0, 0);
                    }
                    else
                    {
                        transform.Rotate(xRotation, 0, 0);
                    }
                }
            }
        }
    }

    private void SmoothIt(Vector3 whereTo)
    {
        Vector3 effectivePanSpeed = whereTo;

        if (adaptToTerrainHeight)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward) * 5, out hit, 5, groundLayer))
            {
                float height = transform.position.y - hit.point.y;
                if (height < zoomMin)
                {
                    whereTo.y += zoomMin - height;
                    whereTo.x = 0;
                    whereTo.z = 0;
                }
            }
        }

        if (smoothing)
        {
            effectivePanSpeed = Vector3.Lerp(lastPanSpeed, whereTo, smoothingFactor);
            lastPanSpeed = effectivePanSpeed;
        }

        cameraTarget = cameraTarget + transform.TransformDirection(effectivePanSpeed) * panSpeed * Time.deltaTime;
        cameraPosition = transform.position;
    }
    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTarget, 10);
    }


    #region Helpers

    public static bool KeyboardInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
            return true;
        else return false;
    }

    private bool MouseInput()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2) || Input.GetAxis("Mouse ScrollWheel") != 0)
            return true;
        else return false;
    }

    private bool MouseOverScreenEdge()
    {
        if (Input.mousePosition.x < mouseEdgeBoundary ||
            Input.mousePosition.x > Screen.width - mouseEdgeBoundary ||
            Input.mousePosition.y < mouseEdgeBoundary ||
            Input.mousePosition.y > Screen.height - mouseEdgeBoundary)
            return true;
        else return false;
    }

    #endregion
}