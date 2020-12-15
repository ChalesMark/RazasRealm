using UnityEngine.Events;
using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

public class BulletCollision : MonoBehaviour
{
    public static event EventHandler<BulletCollisionEventArgs> OnBulletCollision;
    public TextMeshPro damageNumbers;
    private DamageController damageController;
    private List<string> ignoreCollision = new List<string>() { "Player", "Pit", "Bullet", "Pickup" };

    private void Start() {
        damageController = GetComponent<DamageController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            print("Owie :c");
            damageNumbers.text = damageController.Damage(other.GetComponent<HealthController>()).ToString();
            Instantiate(damageNumbers, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - transform.position));
            other.GetComponent<BaddieController>().Bleed();
            Destroy(gameObject);
        }
        else if (!ignoreCollision.Contains(other.tag))
            Destroy(gameObject);
        else
            print(other);
    }
}