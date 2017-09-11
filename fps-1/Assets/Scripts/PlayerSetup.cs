using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
  
    [SerializeField] private Behaviour[] componentsToDisable;
    private Camera sceneCamera;
    [SerializeField] private string remoteLayerName = "RemotePlayer";

    
    void Start()
    {
        if (!this.isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            this.sceneCamera = Camera.main;

            if (this.sceneCamera != null)
                this.sceneCamera.gameObject.SetActive(false);
        }

        RegisterPlayer();

        GetComponent<Player>().Setup();
    }

    void OnDisable()
    {
        if (this.sceneCamera != null)
            this.sceneCamera.gameObject.SetActive(true);

        GameManager.DeregisterPlayer(GetComponent<NetworkIdentity>().netId.Value);
    }

    void DisableComponents()
    {
        for (int i = 0; i < this.componentsToDisable.Length; i++)
        {
            this.componentsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        this.gameObject.layer = LayerMask.NameToLayer(this.remoteLayerName);

        Transform[] children = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.gameObject.layer = LayerMask.NameToLayer(this.remoteLayerName);
        }
    }

    void RegisterPlayer()
    {
        string id = "Player " + GetComponent<NetworkIdentity>().netId;
        this.transform.name = id;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        uint netId = GetComponent<NetworkIdentity>().netId.Value;
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
    }
}
