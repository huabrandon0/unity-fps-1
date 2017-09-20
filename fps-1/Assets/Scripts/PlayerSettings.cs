using UnityEngine;

public class PlayerSettings : MonoBehaviour {

    [SerializeField] private FPCamera cameraScript;

    [SerializeField] private FloatEditor sensEditor;
    [SerializeField] private string sensLabel = "Sensitivity";
    [SerializeField] private float sensMin = 0f;
    [SerializeField] private float sensMax = 5f;
    [SerializeField] private float sensDefault = 0.5f;

    [SerializeField] private FloatEditor fovEditor;
    [SerializeField] private string fovLabel = "FOV";
    [SerializeField] private float fovMin = 10f;
    [SerializeField] private float fovMax = 90f;
    [SerializeField] private float fovDefault = 65f;

    [SerializeField] private FloatEditor zoomSensEditor;
    [SerializeField] private string zoomSensLabel = "Zoom Sensitivity";
    [SerializeField] private float zoomSensMin = 0f;
    [SerializeField] private float zoomSensMax = 5f;
    [SerializeField] private float zoomSensDefault = 0.2f;

    [SerializeField] private FloatEditor zoomFovEditor;
    [SerializeField] private string zoomFovLabel = "Zoom FOV";
    [SerializeField] private float zoomFovMin = 10f;
    [SerializeField] private float zoomFovMax = 90f;
    [SerializeField] private float zoomFovDefault = 25f;
    

    void Start()
    {
        this.sensEditor.SetLabel(this.sensLabel);
        this.sensEditor.SetBounds(this.sensMin, this.sensMax);
        SetSens(this.sensDefault);
        this.sensEditor.AddListeners(SetSens, SetSens);

        this.fovEditor.SetLabel(this.fovLabel);
        this.fovEditor.SetBounds(this.fovMin, this.fovMax);
        SetFov(this.fovDefault);
        this.fovEditor.AddListeners(SetFov, SetFov);

        this.zoomSensEditor.SetLabel(this.zoomSensLabel);
        this.zoomSensEditor.SetBounds(this.zoomSensMin, this.zoomSensMax);
        SetZoomSens(this.zoomSensDefault);
        this.zoomSensEditor.AddListeners(SetZoomSens, SetZoomSens);

        this.zoomFovEditor.SetLabel(this.zoomFovLabel);
        this.zoomFovEditor.SetBounds(this.zoomFovMin, this.zoomFovMax);
        SetZoomFov(this.zoomFovDefault);
        this.zoomFovEditor.AddListeners(SetZoomFov, SetZoomFov);
    }
    
    public void SetSens(float val)
    {
        if (this.sensEditor.SetValue(val))
        {
            this.cameraScript.Sensitivity = val;
        }
    }

    public void SetSens(string str)
    {
        float strVal;
        if (float.TryParse(str, out strVal))
        {
            SetSens(strVal);
        }
        else
        {
            this.sensEditor.RefreshUI();
        }
    }

    public void SetFov(float val)
    {
        if (this.fovEditor.SetValue(val))
        {
            this.cameraScript.Fov = val;
        }
    }

    public void SetFov(string str)
    {
        float strVal;
        if (float.TryParse(str, out strVal))
        {
            SetFov(strVal);
        }
        else
        {
            this.fovEditor.RefreshUI();
        }
    }

    public void SetZoomSens(float val)
    {
        if (this.zoomSensEditor.SetValue(val))
        {
            this.cameraScript.ZoomSensitivity = val;
        }
    }

    public void SetZoomSens(string str)
    {
        float strVal;
        if (float.TryParse(str, out strVal))
        {
            SetZoomSens(strVal);
        }
        else
        {
            this.zoomSensEditor.RefreshUI();
        }
    }

    public void SetZoomFov(float val)
    {
        if (this.zoomFovEditor.SetValue(val))
        {
            this.cameraScript.ZoomFov = val;
        }
    }

    public void SetZoomFov(string str)
    {
        float strVal;
        if (float.TryParse(str, out strVal))
        {
            SetZoomFov(strVal);
        }
        else
        {
            this.zoomFovEditor.RefreshUI();
        }
    }
}
