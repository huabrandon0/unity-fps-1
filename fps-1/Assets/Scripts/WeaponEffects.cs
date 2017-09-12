using UnityEngine;

public class WeaponEffects : MonoBehaviour {

    public ParticleSystem muzzleFlash = null;
    public GameObject bulletImpact = null;

    void Awake()
    {
        if (this.muzzleFlash == null)
        {
            Debug.LogError(GetType() + ": no muzzle flash particle system assigned");
        }

        if (this.bulletImpact == null)
        {
            Debug.LogError(GetType() + ": no bullet impact particle system assigned");
        }
    }
}
