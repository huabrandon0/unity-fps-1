using UnityEngine;
using System.Collections;


// Usage: this script is meant to be placed on a Camera.
// Note: this script makes changes to Transform as if its neutral position and rotation are both (0, 0, 0).
// This means any mouse-look-type script must make changes to the parent of the Camera this is placed on, rather than the Camera itself.
public class FPLean : MonoBehaviour {

    private bool isLeaningLeft = false;
    private bool isLeaningRight = false;
    private bool targetOrientationHasChanged = false;
    private Coroutine leanCoroutine = null;


    [SerializeField] private float leanSpeed = 5f;              // Scales the speed at which the Player leans
    [SerializeField] private float leanDistance = 0.4f;         // How much the Player's Camera is displaced horizontally when leaning
    [SerializeField] private float leanRotationDegrees = 15f;   // How much the Player's Camera rotates when leaning

    
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 currentPos;
    private Vector3 targetPos;

    
    private Quaternion leftRot;
    private Quaternion rightRot;
    private Quaternion currentRot;
    private Quaternion targetRot;


    void Start()
    {
        this.leftPos = Vector3.left * this.leanDistance;
        this.rightPos = Vector3.right * this.leanDistance;
        
        this.leftRot = Quaternion.Euler(Vector3.forward * this.leanRotationDegrees);
        this.rightRot = Quaternion.Euler(Vector3.back * this.leanRotationDegrees);
    }

    void Update()
    {
        if (InputManager.GetKeyDown("Lean Left"))
            LeanLeft();
        else if (InputManager.GetKeyDown("Lean Right"))
            LeanRight();

        if (this.targetOrientationHasChanged)
        {
            // The coroutine to lean must be stopped if still running
            if (this.leanCoroutine != null)
                StopCoroutine(this.leanCoroutine);   // This could possibly attempt to stop a coroutine after it has ended. Bad practice?
            this.leanCoroutine = StartCoroutine("GoToTargetOrientation");
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
