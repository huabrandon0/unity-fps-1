using UnityEngine;
using System.Collections;


// Usage: this script is meant to be placed on a Player.
// The Player must have a CharacterController component.
// The Player must have a FPControllerBhop script to change friction constants. 
[RequireComponent(typeof(CharacterController))]
public class FPStances : MonoBehaviour {

    private FPControllerBhop fpControllerBhop;
    private CharacterController characterController;


    private bool isCrouching = false;
    private bool targetStanceHasChanged = false;
    private Coroutine stanceCoroutine = null;


    [SerializeField] private float crouchSpeed = 8f;    // Scales the speed at which the player transitions between stances

    
    private float standHeight;
    private float crouchHeight;
    private float targetHeight;


    void Awake()
    {
        this.fpControllerBhop = GetComponent<FPControllerBhop>();
        this.characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        this.standHeight = this.characterController.height;
        this.crouchHeight = this.standHeight / 2f;
        this.targetHeight = this.standHeight;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            Uncrouch();

        if (this.targetStanceHasChanged)
        {
            // The coroutine to change stances must be stopped if still running
            if (this.stanceCoroutine != null)
                StopCoroutine(this.stanceCoroutine);   // This could possibly attempt to stop a coroutine after it has ended. Bad practice?
            this.stanceCoroutine = StartCoroutine("GoToTargetStance");
        }
    }

    void Crouch()
    {
        if (!this.isCrouching)
        {
            this.targetStanceHasChanged = true;
            this.isCrouching = true;
            this.targetHeight = this.crouchHeight;
            this.fpControllerBhop.SetFriction(1);
        }
    }

    void Uncrouch()
    {
        if (this.isCrouching)
        {
            this.targetStanceHasChanged = true;
            this.isCrouching = false;
            this.targetHeight = this.standHeight;
            this.fpControllerBhop.SetFriction(0);
        }
    }

    IEnumerator GoToTargetStance()
    {
        this.targetStanceHasChanged = false;

        float startHeight = this.characterController.height;
        float startTime = Time.time;
        float interpVal = 0f;

        while (interpVal < 1f)
        {
            yield return null;

            // Update the interpolation value used for lerp
            interpVal = (Time.time - startTime) * this.crouchSpeed;

            // Store the Player's position and height before any stance changes
            Vector3 lastPos = this.transform.position;
            float lastHeight = this.characterController.height;

            // Adjust the Player's height at a constant rate
            this.characterController.height = Mathf.Lerp(startHeight, this.targetHeight, interpVal);
            
            // Adjust the Player's position such that bottom of the player remains at the same height
            lastPos[1] += (this.characterController.height - lastHeight) * 0.5f;
            this.transform.position = lastPos;
        }
    }
}
