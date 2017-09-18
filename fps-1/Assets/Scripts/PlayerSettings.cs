using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour{
    
    [SerializeField] private FPCamera cameraScript;

    [SerializeField] private Slider mouseSensSlider;
    [SerializeField] private Text mouseSensInputText;
    
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Text fovInputText;

    [SerializeField] private float minSensitivity = 0f;
    [SerializeField] private float maxSensitivity = 4f;
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float maxFOV = 90f;


    public void SetMouseSensitivity(float sens)
    {
        if (sens >= this.minSensitivity && sens <= this.maxSensitivity)
        {
            this.cameraScript.SetMouseSensitivity(sens);
        }
        else
        {
            // not in valid range
        }

        // update UI
    }

    public void SetMouseSensitivity(string sensString)
    {
        SetMouseSensitivity(float.Parse(sensString));
    }

    public void SetFOV(float fov)
    {
        if (fov >= this.minFOV && fov <= this.maxFOV)
        {
            this.cameraScript.SetFOV(fov);
        }
        else
        {
            // not in valid range
        }

        // update UI
    }

    public void SetFOV(string fovString)
    {
        SetFOV(float.Parse(fovString));
    }
}
