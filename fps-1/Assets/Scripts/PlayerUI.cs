using UnityEngine;


// Usage: this script is meant to be placed on a UI object.
// The UI object must be a child of the Player.
public class PlayerUI : MonoBehaviour {

    private bool isPaused;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private FPCamera cameraScript = null;


    void Awake()
    {
        if (this.pauseMenu == null)
        {
            Debug.LogError(GetType() + ": No pause menu object assigned");
            this.enabled = false;
        }

        if (this.cameraScript == null)
        {
            Debug.LogError(GetType() + ": No camera script assigned");
            this.enabled = false;
        }
    }
    
	void Start ()
    {
        this.isPaused = false;
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.T))
        {
            if (!this.isPaused)
            {
                PauseScreen();
                this.isPaused = true;
            }
            else
            {
                UnpauseScreen();
                this.isPaused = false;
            }
        }
	}

    void PauseScreen()
    {
        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable Player's Camera look
        this.cameraScript.SetLockCam(true);

        // Enable pause menu
        this.pauseMenu.SetActive(true);
    }

    void UnpauseScreen()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable Player's Camera look
        this.cameraScript.SetLockCam(false);

        // Disable pause menu
        this.pauseMenu.SetActive(false);
    }
}
