// This class is a base class for all scripts that deal with player input. There are two
// reasons why we use this as a base:

// 1) The protocol for respawning a player is simply disabling (on death) and enabling (on
// respawn) the scripts attached to the player. Having the GetDefaultState() and
// SetDefaultState() methods makes it easy to set the player back to their original state
// upon respawning.

// 2) The ability to stop player inputs is useful. For example, it would be useful to disable
// camera movement while in menus. Having control over which inputs to disable in certain
// situations is very useful (e.g. looting in PUBG/H1Z1: disabling camera movement but allowing
// player movement allows the player to feels less vulnerable).

using UnityEngine.Networking;

abstract public class TakesPlayerInput : NetworkBehaviour {

    // Should be used in GetInput() to stop the polling of physical input
    protected bool canReadInput = true;
    
    public void DisableInput()
    {
        this.canReadInput = false;
        ClearInput();
    }

    public void EnableInput()
    {
        this.canReadInput = true;
    }

    public bool GetCanReadInput()
    {
        return this.canReadInput;
    }

    // Poll input state; assign updated values to input member variables
    abstract protected void GetInput();

    // Clear input state; assign neutral (no physical input) values to input member variables
    abstract protected void ClearInput();

    // Store the script's original state; should be called in Awake()
    abstract protected void GetDefaultState();

    // Reset the script to its original state; should be called in OnEnable()
    abstract protected void SetDefaultState();
}
