using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class Gun : MonoBehaviour
    {
        public float bulletSpeed;
        public float numberOfBullets;
        public float bulletSpread;
        public float firingSpeed;
        public bool autoFire;
        public float bulletLifeSpan;
        public float randomBulletLifeSpan;
        public GameObject bullet;
        public int baseDamage;

        private void Start()
        {
            bullet.GetComponent<Bullet>().AcceptVariables(bulletLifeSpan,randomBulletLifeSpan, bulletSpeed, baseDamage);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<PlayerController>().heldGun = this.gameObject;
            }
        }
    }
}
