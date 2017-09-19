// Usage: this script is meant to be placed on a Camera.

// This script makes changes to the Transform as if its neutral position and rotation are both
// (0, 0, 0). This means any other script that wants to change the Camera's Transform must make
// changes to the parent Transform of the Camera this is placed on, rather than the Camera
// Transform itself.

using UnityEngine;
using System.Collections;

public class FPLean : TakesPlayerInput {

    // Input state
    private bool leanLeft = false;
    private bool leanRight = false;
    
    // Inconstant member variables
    private bool isLeaningLeft = false;
    private bool isLeaningRight = false;
    private bool targetOrientationHasChanged = false;
    private Coroutine leanCoroutine = null;
    
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 targetPos;
    
    private Quaternion leftRot;
    private Quaternion rightRot;
    private Quaternion targetRot;
    
    // Constant member variables
    [SerializeField] private float leanSpeed = 5f;              // Scales the speed at which the Player leans
    [SerializeField] private float leanDistance = 0.4f;         // How much the Player's Camera is displaced horizontally when leaning
    [SerializeField] private float leanRotationDegrees = 15f;   // How much the Player's Camera rotates when leaning


    protected override void GetInput()
    {
        if (!this.canReadInput)
        {
            return;
        }

        this.leanLeft = InputManager.GetKeyDown("Lean Left");
        this.leanRight = InputManager.GetKeyDown("Lean Right");
    }

    protected override void ClearInput()
    {
        this.leanLeft = false;
        this.leanRight = false;
    }

    protected override void GetDefaultState()
    {
    }

    protected override void SetDefaultState()
    {
        ClearInput();
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;

        this.isLeaningLeft = false;
        this.isLeaningRight = false;
        this.targetOrientationHasChanged = false;
        if (this.leanCoroutine != null)
        {
            StopCoroutine(this.leanCoroutine);
            this.leanCoroutine = null;
        }

        this.leftPos = Vector3.left * this.leanDistance;
        this.rightPos = Vector3.right * this.leanDistance;
        this.targetPos = Vector3.zero;

        this.leftRot = Quaternion.Euler(Vector3.forward * this.leanRotationDegrees);
        this.rightRot = Quaternion.Euler(Vector3.back * this.leanRotationDegrees);
        this.targetRot = Quaternion.identity;
    }

    void Awake()
    {
        GetDefaultState();
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

        if (this.leanLeft)
        {
            LeanLeft();
        }
        else if (this.leanRight)
        {
            LeanRight();
        }

        if (this.targetOrientationHasChanged)
        {
            // The coroutine to lean must be stopped if still running
            if (this.leanCoroutine != null)
            {
                StopCoroutine(this.leanCoroutine);  // This could possibly attempt to stop a coroutine after it has ended. Bad practice?
            }

            this.leanCoroutine = StartCoroutine(GoToTargetOrientation());
        }
    }

    void LeanLeft()
    {
        this.targetOrientationHasChanged = true;

        if (this.isLeaningLeft)
        {
            this.isLeaningLeft = false;
            this.isLeaningRight = false;
            this.targetPos = Vector3.zero;
            this.targetRot = Quaternion.identity;
        }
        else
        {
            this.isLeaningLeft = true;
            this.isLeaningRight = false;
            this.targetPos = this.leftPos;
            this.targetRot = this.leftRot;
        }
    }

    void LeanRight()
    {
        this.targetOrientationHasChanged = true;

        if (this.isLeaningRight)
        {
            this.isLeaningRight = false;
            this.isLeaningLeft = false;
            this.targetPos = Vector3.zero;
            this.targetRot = Quaternion.identity;
        }
        else
        {
            this.isLeaningRight = true;
            this.isLeaningLeft = false;
            this.targetPos = this.rightPos;
            this.targetRot = this.rightRot;
        }
    }

    IEnumerator GoToTargetOrientation()
    {
        this.targetOrientationHasChanged = false;

        Vector3 startPos = this.transform.localPosition;
        Quaternion startRot = this.transform.localRotation;
        float startTime = Time.time;
        float interpVal = 0f;

        while (interpVal < 1f)
        {
            yield return null;

            // Update the interpolation value used for lerp
            interpVal = (Time.time - startTime) * this.leanSpeed;
            
            // Adjust the Camera's position and rotation at a constant rate towards the target orientation
            this.transform.localPosition = Vector3.Lerp(startPos, this.targetPos, interpVal);
            this.transform.localRotation = Quaternion.Lerp(startRot, this.targetRot, interpVal);
        }
    }
}
