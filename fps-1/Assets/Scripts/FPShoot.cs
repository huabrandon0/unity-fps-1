using UnityEngine;
using UnityEngine.Networking;


// Usage: this script is meant to be placed on a Player.
// The Player must have a child Camera to shoot from.
// A WeaponManager component must also be present.
[RequireComponent(typeof(WeaponManager))]
public class FPShoot : NetworkBehaviour {

    [SerializeField] private LayerMask maskThatCanBeHit;
    [SerializeField] private Camera camToShootFrom;
    [SerializeField] private string PLAYER_TAG = "Player";

    private Weapon currentWeapon;
    private WeaponManager weaponManager;


    void Awake()
    {
        this.camToShootFrom = GetComponentInChildren<Camera>();
        if (this.camToShootFrom == null)
        {
            Debug.LogError(GetType() + ": Player has no child Camera");
            this.enabled = false;
        }

        this.weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        this.currentWeapon = this.weaponManager.GetCurrentWeapon();

        if (InputManager.GetKeyDown("Attack1"))
            Shoot();
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
}
