using UnityEngine;

[System.Serializable]
public class Weapon {

    public string name = "Pistol";
    public int damage = 10;
    public float range = 200f;
    public int fireRate = 0;
    public GameObject weaponModel = null;

    void Awake()
    {
        if (this.weaponModel == null)
        {
            Debug.LogError(GetType() + ": no weapon model assigned");
        }
    }
}
