using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed;
    public float wallrunSpeed;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public Transform playerCameraHolder;
    public float groundDrag;
    private bool readyToJump = true;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Dashing")]
    public float dashSpeed = 200f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1.0f;
    private float dashCooldownTimer;
    public bool readyToDash = true;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashingKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool isGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    private int i = 1;

    [Header("Wall Handling")]
    public LayerMask wallLayerMask;
    public LayerMask groundLayerMask;
    public float wallCheckDistance = 1;
    private RaycastHit currentWallHit, lastWallHit, groundHit;
    public bool wallrunning;

    [Header("Other")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        dashing,
        wallrunning,
        crouching,
        air
    }

    public bool crouching;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
    }

    private float maxStuckTimeAllowed = 2f, stuckTimer = 0f;
    private Vector3 lastPosition;

    private bool IsStuck()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("lastPositionStored:" + lastPosition);
            //Debug.Log("CurPos:" + transform.position);
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= maxStuckTimeAllowed-1f &&  stuckTimer <= maxStuckTimeAllowed-0.5f)
            {
                lastPosition = transform.position;
            }
            else if (stuckTimer >= maxStuckTimeAllowed && Vector3.Distance(transform.position, lastPosition) <= 0.1f)
            {
                stuckTimer = 0f;
                return true;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        return false;

    }

    private void Update()
    {
        //Stop physics if game is paused.
        Time.timeScale = PauseScript.IsGamePaused ? 0 : 1;
        HandleStuck();
        HandleDrag();
        MyInput();
        SpeedControl();
        CheckForWall();
        StateHandler();
        HandleAnalytics();
    }

    private void HandleStuck()
    {
        if(IsStuck())
            Jump();
    }

    private void HandleAnalytics()
    {
        RaycastHit hit = isGrounded ? groundHit
                            : (wallrunning ? lastWallHit 
                                : default);
        if (hit.point != Vector3.zero)//either ground or wall hit
        {
            AnalyticsManager.Instance.SetLastPlatformTouched(hit.collider.gameObject.name);
        }
                
    }

    private void HandleDrag()
    {
        isGrounded = Physics.Raycast(
                    transform.position, 
                    Vector3.down, 
                    out groundHit, 
                    playerHeight * 0.5f + 0.5f, 
                    whatIsGround);
        rb.linearDamping = (isGrounded || wallrunning) ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        if (!PauseScript.IsGamePaused)
        {
            MovePlayer();
        }
        }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && (isGrounded || wallrunning))
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            crouching = true;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

            crouching = false;
        }
    }

    private void StateHandler()
    {
        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
            lastWallHit = currentWallHit;
        }

        // Mode - Crouching
        else if (crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Dashing 
        else if (!wallrunning && !isGrounded && Input.GetKey(dashingKey) && readyToDash)
        {
            readyToDash = false;
            dashCooldownTimer = dashCooldown;
            Debug.Log("entered" + i++);
            state = MovementState.dashing;
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            rb.AddForce(moveDirection.normalized*dashSpeed*75, ForceMode.Force);
        }

        // Mode - Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desired move speed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());

            print("Lerp Started!");
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        if (dashCooldownTimer <= 0)
        {
            readyToDash = true;
        }
        else
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private IEnumerator DashTimer()
    {
        readyToDash = false;

        yield return new WaitForSeconds(1f);

        readyToDash = true;
        Debug.Log(readyToDash);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction

        rb.useGravity = !wallrunning;
        Transform moveTransform = wallrunning ? playerCameraHolder : orientation;
        moveDirection = moveTransform.forward * verticalInput + moveTransform.right * horizontalInput;
        
        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else// on ground, wallrunning or air
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * (!isGrounded ? airMultiplier : 1), ForceMode.Force);
        }
        // turn gravity off while on slope
        if(!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce((wallrunning ? 
                    (transform.up + lastWallHit.normal).normalized 
                    : transform.up) 
                      * jumpForce, ForceMode.Impulse);
        
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    private void CheckForWall()
    {
        //Check on all sides if you hit a wall and set that to the current wall hit
        RaycastHit rightWall, leftWall, frontWall, backWall;
        bool hitRightWall = Physics.Raycast(transform.position, orientation.right, out rightWall, wallCheckDistance, wallLayerMask);
        bool hitLeftWall = Physics.Raycast(transform.position, -orientation.right, out leftWall, wallCheckDistance, wallLayerMask);
        bool hitFrontWall = Physics.Raycast(transform.position, orientation.forward, out frontWall, wallCheckDistance, wallLayerMask);
        bool hitBackWall = Physics.Raycast(transform.position, -orientation.forward, out backWall, wallCheckDistance, wallLayerMask);

        if (hitRightWall || hitLeftWall || hitFrontWall || hitBackWall)
        {
            wallrunning = true;
            if (hitRightWall)
                currentWallHit = rightWall;
            else if (hitLeftWall)
                currentWallHit = leftWall;
            else if (hitFrontWall)
                currentWallHit = frontWall;
            else if (hitBackWall)
                currentWallHit = backWall;
        }
        else
        {
            wallrunning = false;
        }
    }


}
