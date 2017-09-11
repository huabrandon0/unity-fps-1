using UnityEngine;


// Usage: this script is meant to be placed on a Camera.
// The Camera must be a child of a Player.
public class FPCamera : MonoBehaviour {
    
    private Transform playerTransform;
    private Quaternion defaultPlayerRot;
    private Quaternion defaultCamRot;


    private bool lockCamera;
    [SerializeField] private float sensitivity = 0.5f;
    private float xRotation;
    private float yRotation;
    [SerializeField] private float minimumX = -89f;
    [SerializeField] private float maximumX = 89f;


    void Awake()
    {
        this.playerTransform = this.transform.parent;
        this.defaultPlayerRot = this.playerTransform.localRotation;
        this.defaultCamRot = this.transform.localRotation;
    }

    void OnEnable()
    {
        SetDefaultState();
    }
    
    void Update()
    {
        if (this.lockCamera)
        {
            return;
        }

        // Retrieve mouse input
        this.xRotation -= Input.GetAxis("Mouse Y") * this.sensitivity;
        this.yRotation += Input.GetAxis("Mouse X") * this.sensitivity;

        // Bound x-axis camera rotation
        this.xRotation = Mathf.Clamp(this.xRotation, this.minimumX, this.maximumX);

        // Rotate the player transform around the y-axis, the camera transform around the x-axis
        this.playerTransform.localRotation = Quaternion.Euler(0f, this.yRotation, 0f);

        Vector3 lastRot = this.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(this.xRotation, 0f, lastRot.z);
    }

    void SetDefaultState()
    {
        this.lockCamera = false;
        this.playerTransform.localRotation = this.defaultPlayerRot;
        this.transform.localRotation = this.defaultCamRot;
        this.xRotation = 0f;
        this.yRotation = 0f;
    }

    public void SetLockCam(bool b)
    {
        this.lockCamera = b;
    }
}
