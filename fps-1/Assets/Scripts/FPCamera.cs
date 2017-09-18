// Usage: this script is meant to be placed on a Camera.
// The Camera must be a child of a Player.

using UnityEngine;

public class FPCamera : TakesPlayerInput {
    
    // Input state
    private float xRotation;
    private float yRotation;
    
    // Default state
    private Quaternion defaultPlayerRot;
    private Quaternion defaultCamRot;
    private float defaultXRotation;
    private float defaultYRotation;

    // Inconstant member variables
    private Transform playerTransform;

    // Constant member variables
    [SerializeField] private Camera cam;
    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private float minimumX = -89f;
    [SerializeField] private float maximumX = 89f;


    protected override void GetInput()
    {
        if (!this.canReadInput)
        {
            return;
        }

        // Retrieve mouse input
        this.xRotation -= Input.GetAxis("Mouse Y") * this.sensitivity;
        this.yRotation += Input.GetAxis("Mouse X") * this.sensitivity;
    }

    protected override void ClearInput()
    {
        // The rotation of the camera is an accumulation of previous mouse 
        // inputs, so we cannot "clear" inputs without resetting the rotation.
        // Thus, we have an empty implementation.
    }
    
    protected override void GetDefaultState()
    {
        this.defaultPlayerRot = this.transform.parent.localRotation;
        this.defaultCamRot = this.transform.localRotation;
        this.defaultXRotation = 0f;
        this.defaultYRotation = 0f;
    }

    protected override void SetDefaultState()
    {
        ClearInput();
        this.playerTransform.localRotation = this.defaultPlayerRot;
        this.transform.localRotation = this.defaultCamRot;
        this.xRotation = this.defaultXRotation;
        this.yRotation = this.defaultYRotation;
    }

    void Awake()
    {
        GetDefaultState();
        this.playerTransform = this.transform.parent;
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

        // Bound x-axis camera rotation
        this.xRotation = Mathf.Clamp(this.xRotation, this.minimumX, this.maximumX);

        // Rotate the player transform around the y-axis, the camera transform around the x-axis
        this.playerTransform.localRotation = Quaternion.Euler(0f, this.yRotation, 0f);

        Vector3 lastRot = this.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(this.xRotation, 0f, lastRot.z);
    }

    public void SetMouseSensitivity(float sens)
    {
        this.sensitivity = sens;
    }

    public float GetMouseSensitivity()
    {
        return this.sensitivity;
    }
    
    public void SetFOV(float fov)
    {
        this.cam.fieldOfView = fov;
    }

    public float GetFOV(float fov)
    {
        return this.cam.fieldOfView;
    }
}
