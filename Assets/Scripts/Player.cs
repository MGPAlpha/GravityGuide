using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : GravityObject
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private float wallAngle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    [SerializeField] private UnityEvent eTrigger;

    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;
    private bool facingLeftWall;
    private bool facingRightWall;

    private int facingDirection = 1;

    private bool isGrounded;
    private bool isOnSlope;
    private bool isJumping;
    private bool canWalkOnSlope;
    private bool canJump;

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;

    // private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Aura a;

    // [SerializeField] private Vector2 personalGravity = new Vector2(4,-10);

    [SerializeField] private float auraSize = 5f;
    [SerializeField] private GameObject aura;

    [SerializeField] private Color auraPrimary;
    [SerializeField] private Color auraSecondary;

    [SerializeField] private InteractionManager interactionManager;

    private bool primaryStarted = false;
    private bool secondaryStarted = false;

    [SerializeField] private bool primaryEnabled = true;
    [SerializeField] private bool secondaryEnabled = true;
    [SerializeField] private bool controlsEnabled = true;

    private bool aimSnap = false;
    [SerializeField] private int aimSnapCount = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        base.Start();
        cc = GetComponent<CapsuleCollider2D>();
        a = aura.GetComponent<Aura>();

        capsuleColliderSize = cc.size;
    }

    private Vector2 GetAim() {
        Vector2 aim = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (aimSnap) {
            float angle = Vector2.Angle(aim, Vector2.up);
            if (aim.x < 0) angle = 360 - angle;
            angle = angle * aimSnapCount / (360);
            angle = angle + .5f;
            angle = Mathf.Floor(angle);
            angle = angle * (2*Mathf.PI) / aimSnapCount;
            aim = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        }
        return aim.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlsEnabled) CheckInput();
        // aura.transform.localScale = new Vector3(auraSize, auraSize, 0);
        aura.transform.rotation = Quaternion.Euler (0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
        Vector2 arrowVec = GetAim();
        float arrowDir = Vector2.Angle(arrowVec, Vector2.down);
        if (arrowVec.x > 0) arrowDir = 360 - arrowDir;
        aura.GetComponent<SpriteRenderer>().material.SetFloat("_ArrowAngle", arrowDir * 3.14f / 180);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0,0,Vector2.Angle(personalGravity, Vector2.down) * (personalGravity.x > 0 ? 1 : -1));
        CheckGround();
        SlopeCheck();
        ApplyMovement();
    }

    private float getXInput() {
        xInput = 0;
        float gravityAngle = Vector2.Angle(personalGravity, Vector2.down);
        if (gravityAngle < 67.5) {
            xInput += Input.GetAxisRaw("Horizontal");
        }

        if (gravityAngle > 112.5) {
            xInput += -Input.GetAxisRaw("Horizontal");
        }

        if (gravityAngle > 22.5 && gravityAngle < 157.5) {
            xInput += Input.GetAxisRaw("Vertical") * (personalGravity.x > 0 ? 1 : -1);
        }
        return Mathf.Clamp(xInput, -1, 1);
    }
    
    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) interactionManager.Interact();

        aimSnap = Input.GetKey(KeyCode.LeftShift);
        
        xInput = getXInput();

        if (xInput > 0) GetComponent<SpriteRenderer>().flipX = false;
        if (xInput < 0) GetComponent<SpriteRenderer>().flipX = true;
        GetComponent<Animator>().SetBool("moving", xInput != 0);

        // if (xInput > 0 && facingDirection == -1)
        // {
        //     Flip();
        // }
        // else if (xInput < 0 && facingDirection == 1)
        // {
        //     Flip();
        // }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (secondaryStarted) {
                secondaryStarted = false;
                Time.timeScale = 1;
                a.ActivateAura(false);
            } else if (primaryEnabled) {
                Time.timeScale = .2f;
                primaryStarted = true;
                a.ActivateAura(true);
                a.SetColor(auraPrimary);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            if (primaryStarted) {
                primaryStarted = false;
                Time.timeScale = 1;
                a.ActivateAura(false);
            } else if (secondaryEnabled) {
                Time.timeScale = .2f;
                secondaryStarted = true;
                a.ActivateAura(true);
                a.SetColor(auraSecondary);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (primaryStarted) {
                ActivateGravity(true);
                Time.timeScale = 1;
                primaryStarted = false;
                a.ActivateAura(false);
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            if (secondaryStarted) {
                ActivateGravity(false);
                Time.timeScale = 1;
                secondaryStarted = false;
                a.ActivateAura(false);
            }
        }

    }

    public void EnablePrimary() {
        primaryEnabled = true;
    }
    public void EnableSecondary() {
        secondaryEnabled = true;
    }
    public void DisablePrimary() {
        primaryEnabled = false;
        primaryStarted = false;
    }
    public void DisableSecondary() {
        secondaryEnabled = false;
        secondaryStarted = false;
    }
    public void EnableControls() {
        controlsEnabled = true;
    }
    public void DisableControls() {
        controlsEnabled = false;
    }

    private void ActivateGravity(bool self) {
        Vector2 newGravityDir = GetAim();
        if (self) personalGravity = newGravityDir * personalGravity.magnitude;
        aura.GetComponent<Aura>().AlterGravity(Physics.gravity.magnitude * newGravityDir);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if(Vector2.Dot(rb.velocity, -personalGravity.normalized) <= 0.0f)
        {
            isJumping = false;
        }

        if(isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }

    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - transform.up * capsuleColliderSize.y / 2;

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);


        if (slopeHitFront || slopeHitBack) {

            if (slopeHitFront)
            {
                isOnSlope = true;

                slopeSideAngle = Vector2.Angle(slopeHitFront.normal, transform.up);

                facingRightWall = slopeSideAngle > wallAngle;

            } else {
                facingRightWall = false;
            }
            
            if (slopeHitBack)
            {
                isOnSlope = true;

                slopeSideAngle = Vector2.Angle(slopeHitBack.normal, transform.up);
                facingLeftWall = slopeSideAngle > wallAngle;
            } else {
                facingLeftWall = false;
            }
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
            facingLeftWall = false;
            facingRightWall = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, -transform.up, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            

            slopeDownAngle = Vector2.Angle(hit.normal, -personalGravity.normalized);

            if(slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if ((slopeDownAngle > maxSlopeAngle || !hit) && slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }

    private void Jump()
    {
        if (canJump && canWalkOnSlope)
        {
            canJump = false;
            isJumping = true;
            // newVelocity.Set(0.0f, 0.0f);
            // rb.velocity = newVelocity;
            // newForce.Set(0.0f, jumpForce);
            newForce = jumpForce * -personalGravity.normalized;
            rb.AddForce(newForce, ForceMode2D.Impulse);
        }
    }   

    private void ApplyMovement()
    {
        // rb.velocity += gravity * Time.fixedDeltaTime;
        
        /*if (isGrounded && !isOnSlope && !isJumping) //if not on slope
        {
            Debug.Log("This one");
            // newVelocity.Set(movementSpeed * xInput, 0.0f);
            newVelocity = movementSpeed * xInput * Vector2.Perpendicular(gravity.normalized);
            rb.velocity = newVelocity;
        }
        else*/ if (isGrounded && /*isOnSlope &&*/ canWalkOnSlope && !isJumping) //If on slope
        {
            // newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            newVelocity = slopeNormalPerp * -xInput * movementSpeed;
            rb.velocity = newVelocity;
        }
        else /*if (isGrounded && !canWalkOnSlope && !isJumping) {
            newVelocity = Vector2.Dot(rb.velocity, gravity.normalized) * gravity.normalized;
            rb.velocity = newVelocity;
            rb.velocity += gravity * Time.fixedDeltaTime;
            Debug.Log("Slope slide");
        }
        else*/ if (/*!isGrounded*/ true) //If in air
        {
            float wallAdjustedXInput = xInput;
            
            if (facingLeftWall){
                wallAdjustedXInput = Mathf.Clamp(wallAdjustedXInput, 0,1);
            }
            if (facingRightWall){
                wallAdjustedXInput = Mathf.Clamp(wallAdjustedXInput, -1,0);
            }

            // newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
            
            // rb.velocity = newVelocity;

            float rotatedXVel = Vector2.Dot(rb.velocity, Vector2.Perpendicular(personalGravity.normalized));
            float newRotatedXVel = rotatedXVel;
            if (wallAdjustedXInput == 0) {
                newRotatedXVel = Mathf.MoveTowards(newRotatedXVel, 0, movementSpeed * Time.fixedDeltaTime * 5 / 3);
                Debug.Log(newRotatedXVel);
            } else {
                newRotatedXVel = newRotatedXVel + movementSpeed * wallAdjustedXInput * Time.fixedDeltaTime * 5;
                newRotatedXVel = Mathf.Clamp(newRotatedXVel, -movementSpeed, movementSpeed);
            }


            newVelocity = Vector2.Dot(rb.velocity, personalGravity.normalized) * personalGravity.normalized + newRotatedXVel * Vector2.Perpendicular(personalGravity.normalized);

            rb.velocity = newVelocity;

            // rb.velocity += movementSpeed * wallAdjustedXInput * Vector2.Perpendicular(personalGravity.normalized) * Time.fixedDeltaTime * 10;

            rb.velocity += personalGravity * Time.fixedDeltaTime;
        }

    }

    private void Flip()
    {
        Debug.Log("flippyflip");
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
