using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    // Start is called before the first frame update
    public int BaseDamage;

    public int minBonusDmg;

    public int maxBonusDmg;

    public int Damage(HealthController healthController) {
        //play a hit/collide particle effect or something idk
        int dmg = BaseDamage + UnityEngine.Random.Range(minBonusDmg, maxBonusDmg + 1);
        healthController.DecreaseCurrentHealth(dmg);
        return dmg;
    }

}
