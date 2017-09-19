using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour{
    
    [SerializeField] private FPCamera cameraScript;

    [SerializeField] private Slider sensSlider;
    [SerializeField] private InputField sensInputField;
    
    [SerializeField] private Slider fovSlider;
    [SerializeField] private InputField fovInputField;

    [SerializeField] private Slider zoomSensSlider;
    [SerializeField] private InputField zoomSensInputField;
    
    [SerializeField] private Slider zoomFovSlider;
    [SerializeField] private InputField zoomFovInputField;

    [SerializeField] private float minSensitivity = 0f;
    [SerializeField] private float maxSensitivity = 4f;
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float maxFOV = 90f;
    [SerializeField] private float minZoomSensitivity = 0f;
    [SerializeField] private float maxZoomSensitivity = 4f;
    [SerializeField] private float minZoomFOV = 40f;
    [SerializeField] private float maxZoomFOV = 90f;


    public void SetSensitivity(float val)
    {
        if (val >= this.minSensitivity && val <= this.maxSensitivity)
        {
            this.cameraScript.Sensitivity = val;
        }
        else
        {
            val = this.cameraScript.Sensitivity;
        }

        UpdateUI(this.sensSlider, this.sensInputField, val);
    }

    public void SetSensitivity(string valString)
    {
        if (valString == "")
        {
            UpdateUI(this.sensSlider, this.sensInputField, this.cameraScript.Sensitivity);
            return;
        }

        SetSensitivity(float.Parse(valString));
    }

    public void SetFOV(float val)
    {
        if (val >= this.minFOV && val <= this.maxFOV)
        {
            this.cameraScript.Fov = val;
        }
        else
        {
            val = this.cameraScript.Fov;
        }

        UpdateUI(this.fovSlider, this.fovInputField, val);
    }

    public void SetFOV(string valString)
    {
        if (valString == "")
        {
            UpdateUI(this.fovSlider, this.fovInputField, this.cameraScript.Fov);
            return;
        }

        SetFOV(float.Parse(valString));
    }

    public void SetZoomSensitivity(float val)
    {
        if (val >= this.minZoomSensitivity && val <= this.maxZoomSensitivity)
        {
            this.cameraScript.ZoomSensitivity = val;
        }
        else
        {
            val = this.cameraScript.ZoomSensitivity;
        }

        UpdateUI(this.zoomSensSlider, this.zoomSensInputField, val);
    }

    public void SetZoomSensitivity(string valString)
    {       
        if (valString == "")
        {
            UpdateUI(this.zoomSensSlider, this.zoomSensInputField, this.cameraScript.ZoomSensitivity);
            return;
        }

        SetZoomSensitivity(float.Parse(valString));
    }

    public void SetZoomFOV(float val)
    {
        if (val >= this.minZoomFOV && val <= this.maxZoomFOV)
        {
            this.cameraScript.ZoomFov = val;
        }
        else
        {
            val = this.cameraScript.ZoomFov;
        }

        UpdateUI(this.zoomFovSlider, this.zoomFovInputField, val);
    }

    public void SetZoomFOV(string valString)
    {
        if (valString == "")
        {
            UpdateUI(this.zoomFovSlider, this.zoomFovInputField, this.cameraScript.ZoomFov);
            return;
        }

        SetZoomFOV(float.Parse(valString));
    }

    private void UpdateUI(Slider slider, InputField inputField, float val)
    {
        UpdateSlider(slider, val);
        UpdateInputField(inputField, val);
    }

    private void UpdateSlider(Slider slider, float val)
    {
        slider.value = val;
    }

    private void UpdateInputField(InputField inputField, float val)
    {
        inputField.text = val.ToString();
    }
}
