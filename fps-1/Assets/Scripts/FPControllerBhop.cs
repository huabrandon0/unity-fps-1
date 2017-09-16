// Usage: this script is meant to be placed on a Player.
// The Player must have a CharacterController component.

using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPControllerBhop : TakesPlayerInput {

    // Input state
    private Vector2 inputVec;   // Horizontal movement input
    private bool jump;          // Whether the jump key is inputted

    // Inconstant member variables
    private Vector3 moveVec;                    // Vector3 used to move the character controller
    private float friction;
    private bool isJumping = false;             // Player has jumped and not been grounded yet
    private bool previouslyGrounded = false;    // Player was grounded during the last frame

    // Constant member variables
    private CharacterController characterController;

    [SerializeField] private float gravityMultiplier = 1.6f;
    private float stickToGroundForce = 10f;
    [SerializeField] private float[] frictionConstants = { 5f, 10f };
    [SerializeField] private float jumpSpeed = 5f;  // Initial upwards speed of the jump
    [SerializeField] private float groundAccel = 5f;
    [SerializeField] private float airAccel = 800f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSpeedAir = 1.3f;
    

    protected override void GetInput()
    {
        if (!this.canReadInput)
        {
            return;
        }

        float up = InputManager.GetKey("Strafe Up") ? 1 : 0;
        float left = InputManager.GetKey("Strafe Left") ? -1 : 0;
        float down = InputManager.GetKey("Strafe Down") ? -1 : 0;
        float right = InputManager.GetKey("Strafe Right") ? 1 : 0;
        float h = left + right;
        float v = up + down;
        this.inputVec = new Vector2(h, v);
        
        // TODO: InputManager currently doesn't support scrollwheel input. Must implement that.
        if (!this.jump)
            this.jump = InputManager.GetKeyDown("Jump") || Input.GetAxisRaw("Mouse ScrollWheel") != 0;
    }

    protected override void ClearInput()
    {
        this.inputVec = Vector2.zero;
        this.jump = false;
    }

    protected override void GetDefaultState()
    {
    }

    protected override void SetDefaultState()
    {
        ClearInput();
        this.moveVec = Vector3.zero;
        this.friction = this.frictionConstants[0];
        this.isJumping = false;
        this.previouslyGrounded = false;
    }

    void Awake()
    {
        GetDefaultState();

        this.characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        SetDefaultState();
    }

    public override void OnStartLocalPlayer()
    {
        SetDefaultState();
    }

    void Update()
    {
        GetInput();

        // Jump
        if (!this.previouslyGrounded && this.characterController.isGrounded)
        {
            this.moveVec.y = 0f;
            this.isJumping = false;
        }

        if (!this.characterController.isGrounded && !this.isJumping && this.previouslyGrounded)
        {
            this.moveVec.y = 0f;
        }

        // Horizontal movement
        if (this.inputVec.magnitude > 1)
            this.inputVec = this.inputVec.normalized;

        if (this.characterController.isGrounded)
            MoveGround();
        else
            MoveAir();
        
        //Debug.Log("Speed: " + characterController.velocity.magnitude);
        
        this.characterController.Move(this.moveVec * Time.deltaTime);
        this.jump = false;
        this.previouslyGrounded = this.characterController.isGrounded;
    }
    
    void MoveGround()
    {
        Vector3 wishVel = this.moveSpeed * (transform.forward * this.inputVec.y + this.transform.right * this.inputVec.x);
        Vector3 prevMove = new Vector3(this.moveVec.x, 0, this.moveVec.z);

        // Apply friction
        float speed = prevMove.magnitude;
        if (speed != 0) // To avoid divide by zero errors
        {
            // May implement some "sv_stopspeed"-like variable if low-speed gameplay feels too responsive 
            float drop = speed * this.friction * Time.deltaTime;
            float newSpeed = speed - drop;
            if (newSpeed < 0)
                newSpeed = 0;
            if (newSpeed != speed)
            {
                newSpeed /= speed;
                prevMove = prevMove * newSpeed;
            }

            wishVel -= (1.0f - newSpeed) * prevMove;
        }

        float wishSpeed = wishVel.magnitude;
        Vector3 wishDir = wishVel.normalized;

        Vector3 nextMove = GroundAccelerate(wishDir, prevMove, wishSpeed, this.groundAccel);
        nextMove.y = -this.stickToGroundForce;

        if (this.jump)
        {
            nextMove.y = this.jumpSpeed;
            this.jump = false;
            this.isJumping = true;
        }

        this.moveVec = nextMove;
    }

    Vector3 GroundAccelerate(Vector3 wishDir, Vector3 prevVelocity, float wishSpeed, float accel)
    {
        float currentSpeed = Vector3.Dot(prevVelocity, wishDir);
        float addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0)
            return prevVelocity;

        float accelSpeed = accel * wishSpeed * Time.deltaTime;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        return prevVelocity + accelSpeed * wishDir;
    }

    void MoveAir()
    {
        Vector3 wishVel = this.moveSpeed * (this.transform.forward * this.inputVec.y + transform.right * this.inputVec.x);
        float wishSpeed = wishVel.magnitude;
        Vector3 wishDir = wishVel.normalized;

        Vector3 prevMove = new Vector3(this.moveVec.x, 0, this.moveVec.z);

        Vector3 nextMove = AirAccelerate(wishDir, prevMove, wishSpeed, this.airAccel);
        nextMove.y = this.moveVec.y;
        nextMove += Physics.gravity * this.gravityMultiplier * Time.deltaTime;

        this.moveVec = nextMove;
    }

    Vector3 AirAccelerate(Vector3 wishDir, Vector3 prevVelocity, float wishSpeed, float accel)
    {
        if (wishSpeed > this.maxSpeedAir)
            wishSpeed = this.maxSpeedAir;

        float currentSpeed = Vector3.Dot(prevVelocity, wishDir);
        float addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0)
            return prevVelocity;

        float accelSpeed = accel * wishSpeed * Time.deltaTime;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        return prevVelocity + wishDir * accelSpeed;
    }

    public void SetFriction(int i)
    {
        if (i >= 0 && i < this.frictionConstants.Length)
            this.friction = this.frictionConstants[i];
    }
}
