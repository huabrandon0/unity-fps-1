using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour {
    
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    private Weapon currentWeapon;


    [SerializeField] private Transform weaponBase;
    private GameObject weaponModel;
    [SerializeField] private string viewmodelLayerName = "ViewModel";
    [SerializeField] private string remotePlayerLayerName = "RemotePlayer";


    private WeaponEffects currentWeaponEffects;

    void Start()
    {
        EquipWeapon(this.primaryWeapon);
    }

    void Update()
    {
        if (InputManager.GetKeyDown("Primary Weapon"))
        {
            EquipWeapon(this.primaryWeapon);
        }
        else if (InputManager.GetKeyDown("Secondary Weapon"))
        {
            EquipWeapon(this.secondaryWeapon);
        }
    }

    void EquipWeapon (Weapon weapon)
    {
        this.currentWeapon = weapon;
        Destroy(this.weaponModel); // Note: it may be inefficient to destroy the model entirely (for it can be re-used)
        this.weaponModel = Instantiate(this.currentWeapon.weaponModel) as GameObject;
        this.weaponModel.transform.SetParent(this.weaponBase, false);
        
        // Set the weapon model to the correct layer
        // Why isn't there a cleaner way to set the layers of the children of a GameObject?
        // It seems to imply that there is a more efficient way of setting up the viewmodel than what is currently implemented.
        if (this.isLocalPlayer)
        {
            Util.SetLayersRecursively(this.weaponModel, LayerMask.NameToLayer(this.viewmodelLayerName));
        }
        else
        {
            Util.SetLayersRecursively(this.weaponModel, LayerMask.NameToLayer(this.remotePlayerLayerName));
        }

        this.currentWeaponEffects = this.weaponModel.GetComponent<WeaponEffects>();
        if (this.currentWeaponEffects == null)
        {
            Debug.LogError(GetType() + ": weapon model does not have a WeaponEffects script attached");
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return this.currentWeapon;
    }

    public WeaponEffects GetCurrentWeaponEffects()
    {
        return this.currentWeaponEffects;
    }
}
