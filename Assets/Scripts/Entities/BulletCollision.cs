using UnityEngine.Events;
using UnityEngine;
using System;
using TMPro;
 
public class BulletCollision : MonoBehaviour
{
    public static event EventHandler<BulletCollisionEventArgs> OnBulletCollision;
    public TextMeshPro damageNumbers;
    private DamageController damageController;

    private void Start() {
        damageController = GetComponent<DamageController>();
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.tag == "Enemy")
        {   
            damageNumbers.text = damageController.Damage(other.GetComponent<HealthController>()).ToString();
            Instantiate(damageNumbers, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - transform.position));
            other.GetComponent<BaddieController>().Bleed();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision coll) {
        Destroy(gameObject);
    }
}