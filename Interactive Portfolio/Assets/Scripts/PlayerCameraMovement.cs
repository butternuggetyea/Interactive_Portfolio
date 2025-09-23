using Unity.Burst;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    public Camera _FPCamera;
    [Header("References")]
    public Transform orientation;
    public Transform playerBody;
    public PlayerMovement playerMovement;
    public Camera _handCamera;

    [Header("Rotation")]
    public float mouseSensitivity = 250f;
    public bool invertY = false;
    public float rotationSmoothness = 10f;

    [Header("FOV Effects")]
    public float normalFOV = 80f;
    public float sprintFOV = 85f;
    public float fovChangeSpeed = 10f;

    [Header("Bobbing")]
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public float sprintBobbingMultiplier = 1.5f;

    [Header("Tilt")]
    public float tiltAmount = 5f;
    public float tiltSmoothness = 5f;

    private float xRotation;
    private float mouseX, mouseY;
    private float defaultYPos;
    private float timer;
    private Vector3 currentTilt;
    private Camera cam;
    private bool cameraEnabled = true; // Added camera enabled state

    public Camera PcCamera;

    public static PlayerCameraMovement playerCamera;
    private void Awake()
    {

        playerCamera = this;
        cam = _FPCamera;
        defaultYPos = transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Enables camera movement and effects
    /// </summary>
    public void EnableCamera()
    {
        cameraEnabled = true;
        PcCamera.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Disables camera movement and effects
    /// </summary>
    /// <param name="showCursor">Whether to show cursor when disabled</param>
    public void DisableCamera(bool showCursor = true)
    {
        cameraEnabled = false;
        PcCamera.enabled = true;
        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetMouseSensitivity(float input)
    {
        mouseSensitivity = input;
    }

    [BurstCompile]

    private void FixedUpdate()
    {
        if (!cameraEnabled) return;

        GetInput();
        HandleFOV();
        HandleBobbing();
        HandleRotation();
        HandleTilt();
    }


    private void GetInput()
    {
       
            mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        
    }

    private void HandleRotation()
    {
        // Vertical rotation
        float mouse_y = invertY ? mouseY : -mouseY;
        xRotation = Mathf.Clamp(xRotation + mouse_y * Time.deltaTime, -90f, 90f);

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt.z);
        playerBody.Rotate(Vector3.up * mouseX * Time.deltaTime);
        orientation.rotation = Quaternion.Euler(0, playerBody.eulerAngles.y, 0);
    }

    private void HandleFOV()
    {
        float targetFOV = playerMovement.IsSprinting ? sprintFOV : normalFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovChangeSpeed * Time.deltaTime);
        //_handCamera.fieldOfView = cam.fieldOfView;
    }

    private void HandleBobbing()
    {
        if (!playerMovement.IsGrounded) return;

        float movementMagnitude = new Vector2(playerMovement.HorizontalInput, playerMovement.VerticalInput).magnitude;

        if (movementMagnitude > 0.1f)
        {
            float speed = playerMovement.IsSprinting ?
                walkingBobbingSpeed * sprintBobbingMultiplier :
                walkingBobbingSpeed;

            timer += Time.deltaTime * speed * movementMagnitude;
            Vector3 localPos = transform.localPosition;
            transform.localPosition = new Vector3(
                localPos.x,
                defaultYPos + Mathf.Sin(timer) * bobbingAmount * movementMagnitude,
                localPos.z);
        }
        else
        {
            // Reset position when not moving
            Vector3 localPos = transform.localPosition;
            transform.localPosition = Vector3.Lerp(
                localPos,
                new Vector3(localPos.x, defaultYPos, localPos.z),
                Time.deltaTime * walkingBobbingSpeed);
        }
    }

    private void HandleTilt()
    {
        float tiltTarget = 0f;

        if (Mathf.Abs(playerMovement.HorizontalInput) > 0.1f)
        {
            tiltTarget = -playerMovement.HorizontalInput * tiltAmount;
        }

        currentTilt = Vector3.Lerp(
            currentTilt,
            new Vector3(0, 0, tiltTarget),
            tiltSmoothness * Time.deltaTime);
    }
}
