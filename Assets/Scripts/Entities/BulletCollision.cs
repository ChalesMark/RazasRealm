using UnityEngine.Events;
using UnityEngine;
using System;
using TMPro;
 
public class BulletCollision : MonoBehaviour
{
    //public event EventHandler<BulletCollisionEventArgs> OnBulletCollision;

    public TextMeshPro damageNumbers;

    private void OnTriggerEnter(Collider other)
    {      
        if (other.tag == "Enemy")
        {
            //THIS APPROACH IS CURRENTLY TIGHTLY COUPLED
            //Would be better to post a damage event that can be handled in individual HealthControllers if we want unique damaging mechanics for different types of health controllers
            //This would also allow us for another components to listen for a damage event and execute their own code, splitting up functionality

            //I would suggest a DamageController, A KnockbackController, and a Health Controller on Enemy GameObjects that listen for the BulletCollision event that will be posted below
            other.gameObject.GetComponent<HealthController>().DecreaseCurrentHealth(25);
            damageNumbers.text = "Hit!"; // Replace with actual damage
            Instantiate(damageNumbers, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - transform.position));    

            //further down the line (if we want a enemy piercing buff, need to change this out with something else)
            Destroy(gameObject);

            //This is the commented event, accepts (transform, damage, knockback)

            //OnBulletCollision?.Invoke(this, new BulletCollisionEventArgs(other.transform, 25, 2));      
        }

  
    }
}