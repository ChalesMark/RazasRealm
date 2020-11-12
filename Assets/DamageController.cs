using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    // Start is called before the first frame update
    public int BaseDamage;

    public float DamageRange;

    public int Damage(HealthController healthController) {
        //play a hit/collide particle effect or something idk
        float range = BaseDamage * DamageRange;
        int dmg = BaseDamage + Mathf.RoundToInt(UnityEngine.Random.Range(-range, range));
        healthController.DecreaseCurrentHealth(dmg);
        return dmg;
    }
}
