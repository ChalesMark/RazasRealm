using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Entities
{
    class Bullet : MonoBehaviour
    {
        public float bulletLifeSpan;
        public float bulletSpeed;
        public int baseDamage;
        public TextMeshPro damageNumbers;

        public void AcceptVariables(float bulletLifeSpan,float randomBulletLifeSpan, float bulletSpeed, int baseDamage)
        {
            this.bulletLifeSpan = bulletLifeSpan + UnityEngine.Random.Range(0, randomBulletLifeSpan);
            this.bulletSpeed = bulletSpeed;
            this.baseDamage = 25; //CHANGE TO BASEDAMAGE LATER IDK WHERE ACCEPTVARIABLES IS BEING CALLED            
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
            
            if (other.tag == "Enemy")
            {
                //THIS APPROACH IS CURRENTLY TIGHTLY COUPLED
                //Would be better to post a damage event that can be handled in individual HealthControllers if we want unique damaging mechanics for different types of health controllers
                //This would also allow us for another components to listen for a damage event and execute their own code, splitting up functionality

                //I would suggest a DamageController, A KnockbackController, and a Health Controller on Enemy GameObjects that listen for the BulletCollision event that will be posted below
                other.gameObject.GetComponent<HealthController>().DecreaseCurrentHealth(baseDamage);
                
                damageNumbers.text = baseDamage.ToString();
                Instantiate(damageNumbers, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - transform.position));    

                //further down the line (if we want a enemy piercing buff, need to change this out with something else)
                Destroy(gameObject);
            }
        }
    }
}