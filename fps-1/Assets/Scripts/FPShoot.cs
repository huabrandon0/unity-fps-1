using UnityEngine;
using UnityEngine.Networking;


// Usage: this script is meant to be placed on a Player.
// The Player must have a child Camera to shoot from.
public class FPShoot : NetworkBehaviour {

    [SerializeField] private PlayerWeapon weapon;
    private Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] private string PLAYER_TAG = "Player";


    void Awake()
    {
        this.cam = GetComponentInChildren<Camera>();
        if (this.cam == null)
        {
            Debug.LogError(GetType() + ": Player has no child Camera");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (!this.isLocalPlayer)
        {
            return;
        }

        if (InputManager.GetKeyDown("Attack1"))
            Shoot();
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.cam.transform.position, this.cam.transform.forward, out hit, this.weapon.range, this.mask))
        {
            if (hit.collider.tag == this.PLAYER_TAG)
                CmdPlayerShot(hit.transform.gameObject.GetComponent<NetworkIdentity>().netId.Value, this.weapon.damage);
        }
    }

    [Command]
    void CmdPlayerShot(uint netId, int damage)
    {
        Player player = GameManager.GetPlayer(netId);
        player.RpcTakeDamage(damage);
    }
}
