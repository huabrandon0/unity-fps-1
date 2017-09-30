using UnityEngine;

[RequireComponent(typeof(FPShoot))]
public class WeaponManager : TakesPlayerInput {
    
    // Input state
    private bool switchToPrimaryWeapon;
    private bool switchToSecondaryWeapon;

    // Default state
    private Weapon defaultPrimaryWeapon;
    private Weapon defaultSecondaryWeapon;

    // Inconstant member variables
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    private Weapon currentWeapon;
    private GameObject weaponModel;
    private WeaponEffects weaponEffects;

    // Constant member variables
    private FPShoot shootScript;
    [SerializeField] private Transform weaponBase;
    [SerializeField] private string viewmodelLayerName = "ViewModel";
    [SerializeField] private string remotePlayerLayerName = "RemotePlayer";
    

    protected override void GetInput()
    {
        if (!this.canReadInput)
        {
            return;
        }

        this.switchToPrimaryWeapon = InputManager.GetKeyDown("Primary Weapon");
        this.switchToSecondaryWeapon = InputManager.GetKeyDown("Secondary Weapon");
    }

    protected override void ClearInput()
    {
        this.switchToPrimaryWeapon = false;
        this.switchToSecondaryWeapon = false;
    }

    protected override void GetDefaultState()
    {
        this.defaultPrimaryWeapon = this.primaryWeapon;
        this.defaultSecondaryWeapon = this.secondaryWeapon;
    }

    protected override void SetDefaultState()
    {
        ClearInput();
        this.primaryWeapon = this.defaultPrimaryWeapon;
        this.secondaryWeapon = this.defaultSecondaryWeapon;
        EquipWeapon(this.primaryWeapon);
    }

    void Awake()
    {
        GetDefaultState();

        this.shootScript = GetComponent<FPShoot>();
    }

    void OnEnable()
    {
        SetDefaultState();
    }

    // We override OnStartLocalPlayer because the "isLocalPlayer" property is not valid until it
    // is called. We need to know the true value "isLocalPlayer" to set the weaponModel's layer
    // correctly.
    public override void OnStartLocalPlayer()
    {
        SetDefaultState();
    }

    void Update()
    {
        GetInput();

        if (this.switchToPrimaryWeapon && this.currentWeapon != this.primaryWeapon)
        {
            EquipWeapon(this.primaryWeapon);
            this.shootScript.DisableShooting();
        }
        else if (this.switchToSecondaryWeapon && this.currentWeapon != this.secondaryWeapon)
        {
            EquipWeapon(this.secondaryWeapon);
            this.shootScript.DisableShooting();
        }
        else if (!this.shootScript.CanShoot)
        {
            // Probably should put a timer to disable/enable shooting based on weapon takeout time
            this.shootScript.EnableShooting();
        }
    }
    
    void EquipWeapon (Weapon weapon)
    {
        // Set currentWeapon to new weapon, weaponModel to the new weapon's model
        this.currentWeapon = weapon;
        Destroy(this.weaponModel); // Note: it may be inefficient to destroy the model entirely (it can be re-used!)
        this.weaponModel = Instantiate(this.currentWeapon.weaponModel) as GameObject;
        this.weaponModel.transform.SetParent(this.weaponBase, false);
        
        // Set the newly instantiated weaponModel to the correct layer
        if (this.isLocalPlayer)
        {
            Util.SetLayersRecursively(this.weaponModel, LayerMask.NameToLayer(this.viewmodelLayerName));
        }
        else
        {
            Util.SetLayersRecursively(this.weaponModel, LayerMask.NameToLayer(this.remotePlayerLayerName));
        }

        // Set weaponEffects to the new weapon's WeaponEffects
        this.weaponEffects = this.weaponModel.GetComponent<WeaponEffects>();
        if (this.weaponEffects == null)
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
        return this.weaponEffects;
    }
}
