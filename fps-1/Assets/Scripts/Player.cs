using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

    [SerializeField] private int maxHealth = 100;
    [SyncVar] private int currentHealth;
    [SyncVar] private bool isDead = false;

    public bool IsDead
    {
        get { return this.isDead; }
        protected set { this.isDead = value; }
    }

    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup()
    {
        this.wasEnabled = new bool[this.disableOnDeath.Length];
        for (int i = 0; i < this.wasEnabled.Length; i++)
        {
            this.wasEnabled[i] = this.disableOnDeath[i].enabled;
        }
        
        SetDefaults();
    }

    //void Update()
    //{
    //    if (!this.isLocalPlayer)
    //        return;

    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(99999);
    //    }
    //}

    public void SetDefaults()
    {
        IsDead = false;
        this.currentHealth = this.maxHealth;

        for (int i = 0; i < this.disableOnDeath.Length; i++)
        {
            this.disableOnDeath[i].enabled = this.wasEnabled[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
            characterController.enabled = true;
    }

    [ClientRpc]
    public void RpcTakeDamage(int amt)
    {
        if (IsDead)
            return;

        this.currentHealth -= amt;
        Debug.Log(this.transform.name + " now has " + this.currentHealth);

        if (this.currentHealth <= 0)
        {
            this.currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        // Disable components
        for (int i = 0; i < this.disableOnDeath.Length; i++)
        {
            this.disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        CharacterController characterController = GetComponent<CharacterController>();
        if (characterController != null)
            characterController.enabled = false;

        Debug.Log(this.transform.name + " died");

        // Respawn method
        StartCoroutine(Respawn(GameManager.instance.matchSettings.respawnTime));
    }

    private IEnumerator Respawn(float t)
    {
        yield return new WaitForSeconds(t);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        Debug.Log(this.transform.name + " respawned");
    }
}
