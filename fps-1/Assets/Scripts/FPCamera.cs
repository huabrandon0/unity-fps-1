﻿// Usage: this script is meant to be placed on a Camera.
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
    private float sens;
    public bool IsZoomed { get; private set; }
    [SerializeField] private GameObject scopeOverlay;
    [SerializeField] private Camera viewmodelCam;

    // Constant member variables
    [SerializeField] private Camera cam;
    [SerializeField] private float minimumX = -89f;
    [SerializeField] private float maximumX = 89f;
    [SerializeField] private float sensitivity = 0.5f;
    public float Sensitivity
    {
        get
        {
            return this.sensitivity;
        }
        set
        {
            this.sensitivity = value;
            RefreshState();
        }
    }
    [SerializeField] private float fov = 65f;
    public float Fov
    {
        get
        {
            return this.fov;
        }
        set
        {
            this.fov = value;
            RefreshState();
        }
    }
    [SerializeField] private float zoomSensitivity = 0.3f;
    public float ZoomSensitivity
    {
        get
        {
            return this.zoomSensitivity;
        }
        set
        {
            this.zoomSensitivity = value;
            RefreshState();
        }
    }
    [SerializeField] public float zoomFov = 50f;
    public float ZoomFov
    {
        get
        {
            return this.zoomFov;
        }
        set
        {
            this.zoomFov = value;
            RefreshState();
        }
    }


    protected override void GetInput()
    {
        if (!this.canReadInput)
        {
            return;
        }

        // Retrieve mouse input
        this.xRotation -= Input.GetAxis("Mouse Y") * this.sens;
        this.yRotation += Input.GetAxis("Mouse X") * this.sens;
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
        Unzoom();
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

    public void Zoom()
    {
        this.IsZoomed = true;
        this.sens = ZoomSensitivity;
        this.cam.fieldOfView = ZoomFov;
        this.scopeOverlay.SetActive(true);
        this.viewmodelCam.enabled = false;
    }

    public void Unzoom()
    {
        this.IsZoomed = false;
        this.sens = Sensitivity;
        this.cam.fieldOfView = Fov;
        this.scopeOverlay.SetActive(false);
        this.viewmodelCam.enabled = true;
    }

    void RefreshState()
    {
        if (this.IsZoomed)
        {
            this.sens = ZoomSensitivity;
            this.cam.fieldOfView = ZoomFov;
        }
        else
        {
            this.sens = Sensitivity;
            this.cam.fieldOfView = Fov;
        }
    }
}
