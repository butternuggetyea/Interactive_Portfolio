using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float groundDrag = 6f;
    public float slopeForce = 10f;
    public float maxSlopeAngle = 45f;

    [Header("Step Climbing")]
    public float maxStepHeight = 0.3f;
    public float stepSearchDistance = 0.1f;
    public float stepForce = 10f;
    public float stepCheckOffset = 0.05f;

    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float airMultiplier = 0.4f;
    public float jumpCooldown = 0.25f;
    public float jumpGravityScale = 2f;
    public float fallGravityScale = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public LayerMask groundMask;
    public LayerMask interactMask;
    public LayerMask interiorGround;
    public float groundCheckDistance = 0.2f;
    public float groundCheckRadius = 0.4f;
    public float wallCheckDistance = 0.6f;

    // Properties
    public bool IsGrounded { get; private set; }
    public bool IsSprinting { get; private set; }
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
    public bool MovementEnabled { get; set; } = true;

    // Private variables
    private float currentSpeed;
    private bool readyToJump = true;
    private bool isJumping;
    private bool exitingSlope;
    private RaycastHit slopeHit;
    private Rigidbody rb;
    private CapsuleCollider col;
    private Transform orientation;
    public static Animator animator;

    public static PlayerMovement playerMovement { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = this;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        orientation = transform.Find("Orientation") ?? transform;
        rb.freezeRotation = true;
    }

    public void EnableMovement()
    {
        MovementEnabled = true;
        col.enabled = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public void DisableMovement()
    {
        MovementEnabled = false;
        col.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void Update()
    {
        if (!MovementEnabled) return;

        GroundCheck();
        GetInput();
        SpeedControl();
        StateHandler();
        ApplyCustomGravity();
        HandleDrag();
    }

    private void FixedUpdate()
    {
        if (!MovementEnabled) return;

        MovePlayer();
        HandleStepClimbing();
    }

    private void GroundCheck()
    {
        bool groundHit = Physics.SphereCast(
            transform.position,
            groundCheckRadius,
            Vector3.down,
            out _,
            groundCheckDistance,
            groundMask
        );

        if (!groundHit)
        {
            groundHit = Physics.SphereCast(
                transform.position,
                groundCheckRadius,
                Vector3.down,
                out _,
                groundCheckDistance,
                interactMask
            );
        }

        if (!groundHit)
        {
            groundHit = Physics.SphereCast(
                transform.position,
                groundCheckRadius,
                Vector3.down,
                out _,
                groundCheckDistance,
                interiorGround
            );
        }

        IsGrounded = groundHit;
    }

    private void GetInput()
    {
        if (!Application.isFocused) return;

        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        if (HorizontalInput == 0 && VerticalInput == 0)
        {

            animator.SetBool("Walking", false);
        }
        else
        {
            animator.SetBool("Walking", true);
        }

        if (Input.GetButton("Jump") && readyToJump && IsGrounded)
        {
            readyToJump = false;
            isJumping = true;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    private void StateHandler()
    {
        IsSprinting = Input.GetKey(KeyCode.LeftShift) && VerticalInput > 0 && IsGrounded;
        currentSpeed = IsSprinting ? sprintSpeed : walkSpeed;
    }

    private void MovePlayer()
    {
        if (AgainstWall() && !exitingSlope) return;

        Vector3 moveDirection = orientation.forward * VerticalInput + orientation.right * HorizontalInput;
        moveDirection = moveDirection.normalized;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * currentSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * slopeForce, ForceMode.Force);
        }
        else if (IsGrounded)
        { 
            rb.AddForce(moveDirection * currentSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection * currentSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope() && !IsGrounded;
    }

    private void HandleStepClimbing()
    {
        if (!IsGrounded || rb.linearVelocity.magnitude < 0.1f) return;

        Vector3 moveDirection = (orientation.forward * VerticalInput + orientation.right * HorizontalInput).normalized;
        if (moveDirection.magnitude < 0.1f) return;

        Vector3 rayOrigin = transform.position + Vector3.up * stepCheckOffset;
        float rayDistance = col.radius + stepSearchDistance;

        if (Physics.Raycast(rayOrigin, moveDirection, out RaycastHit lowerHit, rayDistance, groundMask))
        {
            Vector3 upperRayStart = lowerHit.point + Vector3.up * maxStepHeight + moveDirection * col.radius;

            if (!Physics.Raycast(upperRayStart, Vector3.down, out RaycastHit upperHit, maxStepHeight, groundMask))
                return;

            float stepHeight = upperHit.point.y - transform.position.y;

            if (stepHeight > 0 && stepHeight <= maxStepHeight)
            {
                rb.AddForce(Vector3.up * stepForce * stepHeight, ForceMode.VelocityChange);
            }
        }
    }

    private void ApplyCustomGravity()
    {
        if (!IsGrounded)
        {
            if (isJumping && rb.linearVelocity.y > 0)
            {
                rb.AddForce(Physics.gravity * (jumpGravityScale - 1) * rb.mass);
            }
            else if (rb.linearVelocity.y < 0)
            {
                rb.AddForce(Physics.gravity * (fallGravityScale - 1) * rb.mass);
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.AddForce(Physics.gravity * (lowJumpMultiplier - 1) * rb.mass);
            }
        }
    }

    private void HandleDrag()
    {
        rb.linearDamping = IsGrounded ? groundDrag : 0f;
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > currentSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (flatVel.magnitude > currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * currentSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, col.height * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private bool AgainstWall()
    {
        Vector3[] directions = {
            orientation.forward,
            -orientation.forward,
            orientation.right,
            -orientation.right
        };

        foreach (Vector3 dir in directions)
        {
            if (Physics.Raycast(transform.position, dir, wallCheckDistance, groundMask))
                return true;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        // Ground check sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - Vector3.up * groundCheckDistance, groundCheckRadius);

        // Step detection rays
        if (Application.isPlaying)
        {
            Vector3 moveDir = (orientation.forward * VerticalInput + orientation.right * HorizontalInput).normalized;
            if (moveDir.magnitude > 0.1f)
            {
                Gizmos.color = Color.blue;
                Vector3 rayOrigin = transform.position + Vector3.up * stepCheckOffset;
                Gizmos.DrawLine(rayOrigin, rayOrigin + moveDir * (col.radius + stepSearchDistance));

                Gizmos.color = Color.green;
                Vector3 upperOrigin = rayOrigin + moveDir * (col.radius + stepSearchDistance) + Vector3.up * maxStepHeight;
                Gizmos.DrawLine(upperOrigin, upperOrigin + Vector3.down * maxStepHeight);
            }
        }
    }
}
