using UnityEngine.Events;
using UnityEngine;
using System;
using TMPro;
 
public class BulletCollision : MonoBehaviour
{
    public static event EventHandler<BulletCollisionEventArgs> OnBulletCollision;
    public TextMeshPro damageNumbers;

    private void OnTriggerEnter(Collider other)
    {      
        if (other.tag == "Enemy")
        {   
            //further down the line (if we want a enemy piercing buff, need to change this out with something else)
            Destroy(gameObject);

            damageNumbers.text = "Hit!"; // Replace with actual damage
            Instantiate(damageNumbers, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - transform.position)); 

            OnBulletCollision?.Invoke(this, new BulletCollisionEventArgs(other.transform, 25, 2));      
        }
    }
}