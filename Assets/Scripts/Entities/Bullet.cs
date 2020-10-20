using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class Bullet : MonoBehaviour
    {
        public float bulletLifeSpan;
        public float bulletSpeed;
        public int baseDamage;
        public void AcceptVariables(float bulletLifeSpan, float bulletSpeed, int baseDamage)
        {
            this.bulletLifeSpan = bulletLifeSpan;
            this.bulletSpeed = bulletSpeed;
            this.baseDamage = baseDamage;
        }

        // Update
        // Runs every frame
        void Update()
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;

            bulletLifeSpan -= 1 * Time.deltaTime;

            if (bulletLifeSpan <= 0)
                Destroy(this.gameObject);            
        }

        // OnTriggerEnter
        // Is called when a collider enters. Used to check if a monster touches
        // Parama:	Collider other:		The other collider that touched this object
        void OnTriggerEnter(Collider other)
        {
            print(other);
            if (other.tag == "Enemy")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}