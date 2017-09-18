// Usage: this script is meant to be placed on a Player.
// The Player must be assigned a Camera to shoot from.
// A WeaponManager component must be present.

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(WeaponManager))]
public class FPShoot : TakesPlayerInput {
    
    // Input state
    private bool shootKeyDown = false;
    private bool shootKeyUp = false;

    // Inconstant member variables
    private bool canShoot = true;
    private bool isShooting = false;
    private Coroutine shootCoroutine = null;
    private Weapon currentWeapon;

    // Constant member variables
    private WeaponManager weaponManager;
    [SerializeField] private LayerMask maskThatCanBeHit;
    [SerializeField] private Camera camToShootFrom;
    [SerializeField] private string PLAYER_TAG = "Player";
    

    protected override void GetInput()
    {
        if (!this.canReadInput)
        {
            return;
        }

        this.shootKeyDown = InputManager.GetKeyDown("Attack1");
        this.shootKeyUp = InputManager.GetKeyUp("Attack1");
    }

    protected override void ClearInput()
    {
        this.shootKeyDown = false;
        this.shootKeyUp = false;
    }

    protected override void GetDefaultState()
    {
    }

    protected override void SetDefaultState()
    {
        ClearInput();
        this.isShooting = false;
        StopShootCoroutine();
        this.currentWeapon = this.weaponManager.GetCurrentWeapon();
    }

    void Awake()
    {
        GetDefaultState();

        if (this.camToShootFrom == null)
        {
            Debug.LogError(GetType() + ": Player has assigned Camera");
            this.enabled = false;
        }

        this.weaponManager = GetComponent<WeaponManager>();
    }

    void OnEnable()
    {
        SetDefaultState();
    }

    void Update()
    {
        GetInput();

        this.currentWeapon = this.weaponManager.GetCurrentWeapon();

        if (this.canShoot)
        {
            if (this.currentWeapon.fireRate <= 0f)
            {
                // Tap fire
                if (this.shootKeyDown)
                {
                    Shoot();
                }
            }
            else
            {
                // Automatic fire
                if (this.shootKeyDown && this.isShooting == false)
                {
                    this.isShooting = true;
                    StartShootCoroutine();
                }
                else if (this.shootKeyUp && this.isShooting == true)
                {
                    this.isShooting = false;
                    StopShootCoroutine();
                }
            }
        }
    }

    [Client]
    void Shoot()
    {
        // Hmm, there is already a client attribute on this function. Is this check necessary?
        if (!this.isLocalPlayer) 
        {
            return;
        }

        CmdOnShoot();

        RaycastHit hit;
        if (Physics.Raycast(this.camToShootFrom.transform.position, this.camToShootFrom.transform.forward, out hit, this.currentWeapon.range, this.maskThatCanBeHit))
        {
            CmdOnHit(hit.point, hit.normal);

            if (hit.collider.tag == this.PLAYER_TAG)
                CmdPlayerShot(hit.transform.gameObject.GetComponent<NetworkIdentity>().netId.Value, this.currentWeapon.damage);
        }
    }

    [Command]
    void CmdPlayerShot(uint netId, int damage)
    {
        Player player = GameManager.GetPlayer(netId);
        player.RpcTakeDamage(damage);
    }

    [Command]
    void CmdOnShoot()
    {
        RpcShootEffects();
    }

    // Tell all clients to produce a shoot effect
    // Note: it may be better practice to call shoot effects with a coroutine
    [ClientRpc]
    void RpcShootEffects()
    {
        this.weaponManager.GetCurrentWeaponEffects().muzzleFlash.Play();
    }
    
    [Command]
    void CmdOnHit (Vector3 pos, Vector3 normal)
    {
        RpcHitEffects(pos, normal);
    }

    // Tell all clients to produce a hit effect
    // Note: look into "object pooling" as a more efficient way of spawning these bullet impacts
    [ClientRpc]
    void RpcHitEffects(Vector3 pos, Vector3 normal)
    {
        GameObject bulletImpact = Instantiate(this.weaponManager.GetCurrentWeaponEffects().bulletImpact, pos, Quaternion.LookRotation(normal));
        Destroy(bulletImpact, 1f);
    }

    private IEnumerator ShootAutomatic(float timeBetweenShots)
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void StartShootCoroutine()
    {
        this.shootCoroutine = StartCoroutine(ShootAutomatic(1f / this.currentWeapon.fireRate));
    }

    private void StopShootCoroutine()
    {
        if (this.shootCoroutine != null)
        {
            StopCoroutine(this.shootCoroutine);
            this.shootCoroutine = null;
        }
    }

    public bool GetCanShoot()
    {
        return this.canShoot;
    }

    public void DisableShooting()
    {
        this.canShoot = false;
        StopShootCoroutine();
    }

    public void EnableShooting()
    {
        this.canShoot = true;
    }
}
