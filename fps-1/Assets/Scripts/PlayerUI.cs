using UnityEngine;


// Usage: this script is meant to be placed on a UI object.
// The UI object must be a child of the Player.
public class PlayerUI : MonoBehaviour {

    private bool isPaused;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private FPCamera cameraScript = null;
    [SerializeField] private Behaviour[] componentsToDisableInMenu;

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
        // FIX LATER: change menu keycode to Esc on the actual build
        // Unity has weird hotkeys built-in the game tab, so we're using "T" in the meantime.
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

        // Disable Player's Camera look, Player's ability to shoot
        this.cameraScript.SetLockCam(true);
        DisableComponents();

        // Enable pause menu
        this.pauseMenu.SetActive(true);
    }

    void UnpauseScreen()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable Player's Camera look, Player's ability to shoot
        this.cameraScript.SetLockCam(false);
        EnableComponents();

        // Disable pause menu
        this.pauseMenu.SetActive(false);
    }

    void DisableComponents()
    {
        for (int i = 0; i < this.componentsToDisableInMenu.Length; i++)
        {
            this.componentsToDisableInMenu[i].enabled = false;
        }
    }

    void EnableComponents()
    {
        for (int i = 0; i < this.componentsToDisableInMenu.Length; i++)
        {
            this.componentsToDisableInMenu[i].enabled = true;
        }
    }
}
