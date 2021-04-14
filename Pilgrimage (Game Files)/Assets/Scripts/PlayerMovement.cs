using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //WallRunning
    public LayerMask whatIsWall;
    public float wallRunForce, maxWallRunTime, maxWallSpeed;
    bool isWallRight, isWallLeft;
    bool isWallRunning;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    //Clamping
    public Vector3 ledgeClimbPos;
    public Vector3 ledgeOrigPos;
    public bool climbing = false;
    public float climbTime = 0.2f;


    private void WallRunInput()
    {
        //Start Wallrun
        if (isWallRight && !grounded)
            StartWallRun();
        if (isWallLeft && !grounded) StartWallRun();

    }
    private void StartWallRun()
    {
        
        rb.useGravity = false;
        isWallRunning = true;
        if(rb.velocity.magnitude <=maxWallSpeed)
        {
            
            rb.AddForce(orientation.forward * wallRunForce * 50 * Time.deltaTime);
            rb.velocity
    = rb.velocity - Vector3.Project(
                rb.velocity, transform.up);

            //Stick to the wall
            if (isWallRight)
                rb.AddForce(orientation.right * wallRunForce / 5 * Time.deltaTime);
            else
                rb.AddForce(-orientation.right * wallRunForce / 5 * Time.deltaTime);
        }

    }
    private void StopWallRun()
    {
        
        rb.useGravity = true;
        isWallRunning = false;
    }
    private void CheckForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

        if (!isWallRight && !isWallLeft) StopWallRun();

    }

    //Assingables
    public Transform playerCam;
    public Transform orientation;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20f;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 20f;
    public float slideCounterMovement = 0.001f;
    public bool isSlide;

    //Jumping
    public bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    //Input
    float x, y;
    bool jumping, sprinting, crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;
    
    //Inventory (Abraham Addition)
    public Inventory inventory;
    //[SerializeField] private UI_Inventory uiInvetory;

    private void OnTriggerEnter(Collider item)
    {
        PickUpController itemWorld = item.GetComponent<PickUpController>();
        if(itemWorld != null)
        {
            //Touching the item
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
            Debug.Log(inventory.itemList.Count);
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        //Abraham addition for inventory system
        inventory = new Inventory();
        //uiInvetory.SetInventory(inventory);
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        PlayerStats.Level = sceneName;


    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        MyInput();
        Look();
        CheckForWall();
        WallRunInput();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
        //Running
        if (Input.GetKeyDown(KeyCode.LeftShift))
            StartRun();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            StopRun();

    }
    private void StartRun()
    {
        maxSpeed =20f;
    }
    private void StopRun()
    {
        maxSpeed = 15f;
    }

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce *maxSpeed);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        if (climbing)
        {
            Vector3 rbVelocity = rb.velocity;
            rbVelocity.y = 0;
            rb.MovePosition(Vector3.Lerp(ledgeOrigPos, ledgeClimbPos + Vector3.up * 2f, climbTime));
            rb.velocity += rbVelocity;
            climbTime += Time.fixedDeltaTime * 10f;
            if (climbTime >= 1.0)
            {
                climbing = false;
            }
        }
        if (isWallRunning)
        {
            grounded = false;
        }
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 12  );

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        
        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            if (x > 0 && xMag > maxSpeed/2) x = 0;
            if (x < 0 && xMag < -maxSpeed/2) x = 0;
            if (y > 0 && yMag > maxSpeed/2) y = 0;
            if (y < 0) y = 0;

            rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime* 1f);
            rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * 1f);
            rb.AddForce(Vector3.down * Time.deltaTime * 6000);

            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (!isWallRunning)
        {
            if (x > 0 && xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (y > 0 && yMag > maxSpeed) y = 0;
            if (y < 0 && yMag < -maxSpeed) y = 0;

        }
        else
        {

            if (x > 0 && xMag > maxSpeed*1.2) x = 0;
            if (x < 0 && xMag < -maxSpeed*1.2) x = 0;
            if (y > 0 && yMag > maxSpeed*1.2) y = 0;
            if (y < 0 && yMag < -maxSpeed*1.2) y = 0;
        }


        // Movement in air
        if (!grounded && !isWallRunning)
        {
            if (x > 0 && xMag > maxSpeed/1.5) x = 0;
            if (x < 0 && xMag < -maxSpeed/1.5) x = 0;
            if (y > 0 && yMag > maxSpeed/1.5) y = 0;
            if (y < 0 && xMag < -maxSpeed / 1.5) y = 0;
            multiplier = 0.5f;
            multiplierV = 0.5f;
            
        }


        // Movement while sliding
        if (crouching)
        {

            multiplier = 0.3f;
        }
        if(isSlide)
        {
            if (x > 0 && xMag > maxSpeed * 100) x = 0;
            if (x < 0 && xMag < -maxSpeed * 100) x = 0;
            if (y > 0 && yMag > maxSpeed * 100) y = 0;
            if (y < 0 && yMag < -maxSpeed *100) y = 0;
        }

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
        if(isWallRunning)
        {

        }
    }

    private void Jump()
    {
        if (grounded && readyToJump && !isWallRunning )
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (isWallRunning)
        {
            readyToJump = false;

            //normal jump


            //sidwards wallhop
            if (isWallRight)
            {

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    rb.velocity = Vector3.zero;
                    rb.AddForce(-orientation.right * jumpForce * 2.2f);
                    rb.AddForce(orientation.up * jumpForce * 2f);
                    rb.AddForce(orientation.forward * 100 * 1.2f);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    rb.AddForce(-orientation.right * jumpForce * 1.2f);
                    rb.AddForce(orientation.up * jumpForce * 1.7f);
                    rb.AddForce(orientation.forward * 100 * 1.2f);

                }

            }

            if (isWallLeft)
            {
                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
                {
                    rb.velocity = Vector3.zero;
                    rb.AddForce(orientation.right * jumpForce * 2.2f);
                    rb.AddForce(orientation.up * jumpForce * 2f);
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        rb.AddForce(orientation.forward * 100 * 1.2f);
                    }
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    rb.AddForce(orientation.right * jumpForce * 1.2f);
                    rb.AddForce(orientation.up * jumpForce * 1.7f);
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        rb.AddForce(orientation.forward * 100 * 1.2f);
                    }
                }
               
            }
           

            //Always add forward force
            rb.AddForce(orientation.forward * jumpForce * 1f);
            maxSpeed = 17f;

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallRunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        //While Wallrunning
        //Tilts camera in .5 second
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }
        
        //Slow down jumping
        if(!grounded)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * 1);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {


        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                isSlide = false;
                slideCounterMovement = 0.001f;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }

        if (climbing || !other.collider.CompareTag("Climbable"))
            return;


        Vector3 normal2 = other.GetContact(0).normal;
        Vector3 horForward = playerCam.transform.forward;
        horForward.y = 0;
        horForward.Normalize();
        if (Vector3.Angle(horForward, -normal2) <= 45)
        {
            bool ledgeAvail = true;
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position + Vector3.up * 0.5f, -normal2, out hit, 1, LayerMask.GetMask("Ground")))
            {
                ledgeAvail = false;
            }
            if (ledgeAvail)
            {
                Vector3 currPos = playerCam.transform.position + Vector3.up * 0.5f + Vector3.down * 0.5f;
                while (!Physics.Raycast(currPos, -normal2, out hit, 1, LayerMask.GetMask("Ground")))
                {
                    currPos += Vector3.down * 0.05f;
                    if (currPos.y < playerCam.transform.position.y - 2f)
                        break;
                }
                ledgeOrigPos = this.transform.position;
                ledgeClimbPos = currPos - normal2;
                climbing = true;
                climbTime = 0.0f;


            }
        }
    }

    private void StopGrounded()
    {
        grounded = false;
        isSlide = true;
        slideCounterMovement = 0f;
    }

}