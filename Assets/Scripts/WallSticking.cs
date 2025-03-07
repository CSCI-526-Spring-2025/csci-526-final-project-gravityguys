using UnityEngine;
using System.Collections;

public class WallSticking : MonoBehaviour
{

    [SerializeField] private LayerMask wallLayerMask, groundLayerMask;

    [SerializeField] private float maxWallRunTime, wallClimbDescendSpeed,
                                   wallJumpUpForce, wallJumpSideForce, exitWallTime;

    [SerializeField] private KeyCode upwardsWallRunKey = KeyCode.LeftShift,
                                     downwardsWallRunKey = KeyCode.LeftControl,
                                     wallJumpKey = KeyCode.Space;

    private float horizontalInput, verticalInput, wallStickingTimer, exitWallTimer;
    private bool upwardsWallRunning, downwardsWallRunning, exitingWall;
    [SerializeField] float wallRunForce = 200f;

    [SerializeField] private float wallCheckDistance, minJumpHeight;
    private RaycastHit rightWallHit, leftWallHit, curWallHit, lastWallHit;
    private bool wallLeft, wallRight;

    [SerializeField] private Transform orientation;
    private PlayerMovement pm;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    void FixedUpdate()
    {
        if (pm.wallrunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position,orientation.right,out rightWallHit,wallCheckDistance,wallLayerMask);
        wallLeft = Physics.Raycast(transform.position,-orientation.right,out leftWallHit,wallCheckDistance,wallLayerMask);
        if(wallRight)
            curWallHit = rightWallHit;
        if(wallLeft)
            curWallHit = leftWallHit;
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position,Vector3.down, minJumpHeight,groundLayerMask);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        upwardsWallRunning = Input.GetKey(upwardsWallRunKey);
        downwardsWallRunning = Input.GetKey(downwardsWallRunKey);

        if (exitingWall && lastWallHit.collider != curWallHit.collider)
        {
            exitingWall = false;
        }

        if ((wallLeft || wallRight) && AboveGround() && verticalInput > 0 && !exitingWall)//wall run
        {
            lastWallHit = curWallHit;
            if (!pm.wallrunning)
            {
                StartWallRun();
            }
            if(wallStickingTimer > 0)
                wallStickingTimer -= Time.deltaTime;
            Debug.Log("wall sticiking: " + wallStickingTimer);
            if (wallStickingTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }
            if (Input.GetKeyDown(wallJumpKey))
                WallJump();
        }else if (exitingWall) //jumping off || stamina has run out
        {
            if (pm.wallrunning)
                EndWallRun();
            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;
            Debug.Log("Exiting Wall: " + exitWallTimer);
            if (exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        else
        {
            if (pm.wallrunning)
            {
                  EndWallRun();
            }
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        wallStickingTimer = maxWallRunTime;
        AnalyticsManager.Instance.SetLastPlatformTouched(curWallHit.collider.gameObject.name);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal,transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward + wallForward).magnitude)
        {
            wallForward = - wallForward;
        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        if(upwardsWallRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, wallClimbDescendSpeed, rb.linearVelocity.z);
        if(downwardsWallRunning)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallClimbDescendSpeed, rb.linearVelocity.z);

        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void EndWallRun()
    {
        pm.wallrunning = false;
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal,
                force2Apply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0.0f, rb.linearVelocity.z);
        rb.AddForce(force2Apply, ForceMode.Impulse);

    }
}
