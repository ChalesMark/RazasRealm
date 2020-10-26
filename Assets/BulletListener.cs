using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Subclass this class if you would like to change behaviour from events
public class BulletListener : MonoBehaviour
{
    // Start is called before the first frame update
    HealthController healthController;


    void Start()
    {
        BulletCollision.OnBulletCollision += TakeDamage;
        //+= Knockback?
        //+= Debuff?

        healthController = GetComponent<HealthController>();

    }

    private void OnDestroy()
    {
        BulletCollision.OnBulletCollision -= TakeDamage;
        //-= Knockback?
        //+= Debuff?
    }

    
    private void TakeDamage(object sender, BulletCollisionEventArgs args) 
    {   
        if(args.Target == this.transform) {
            healthController.DecreaseCurrentHealth(args.Damage);
        }
    }
}
