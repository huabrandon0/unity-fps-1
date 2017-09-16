// Usage: this script is meant to be placed on a UI object.
// The UI object must be a child of the Player.

using UnityEngine;

public class PlayerUI : MonoBehaviour {

    private bool isPaused;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private TakesPlayerInput[] disableInputsWhilePaused;

    void Awake()
    {
        if (this.pauseMenu == null)
        {
            Debug.LogError(GetType() + ": No pause menu object assigned");
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

        // Disable Player's controls
        DisableInputs();

        // Enable pause menu
        this.pauseMenu.SetActive(true);
    }

    void UnpauseScreen()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable Player's controls
        EnableInputs();

        // Disable pause menu
        this.pauseMenu.SetActive(false);
    }

    void DisableInputs()
    {
        for (int i = 0; i < this.disableInputsWhilePaused.Length; i++)
        {
            this.disableInputsWhilePaused[i].DisableInput();
        }
    }

    void EnableInputs()
    {
        for (int i = 0; i < this.disableInputsWhilePaused.Length; i++)
        {
            this.disableInputsWhilePaused[i].EnableInput();
        }
    }
}
