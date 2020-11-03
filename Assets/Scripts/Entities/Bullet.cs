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
        public float bulletSpeed;
        public int baseDamage;
        public string owner;
        public ParticleSystem ps;
        ParticleSystem internalPS;

        private void Start()
        {
            if (ps)
                internalPS = Instantiate(ps);
        }

        public void AcceptVariables(float bulletLifeSpan,float randomBulletLifeSpan, float bulletSpeed, int baseDamage)
        {
            this.bulletSpeed = bulletSpeed;
            this.baseDamage = 25; //CHANGE TO BASEDAMAGE LATER IDK WHERE ACCEPTVARIABLES IS BEING CALLED            
        }

        // Update
        // Runs every frame
        void Update()
        {
            if (internalPS)
                internalPS.transform.position = this.transform.position;
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;   
        }
    }
}