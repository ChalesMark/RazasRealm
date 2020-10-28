using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities
{
    class Gun : MonoBehaviour, IBuyable
    {
        public float bulletSpeed;
        public float numberOfBullets;
        public float bulletSpread;
        public float firingSpeed;
        public bool autoFire;
        public float bulletLifeSpan;
        public GameObject bullet;
        public ShootType AnimationType;
        public int baseDamage;
        public GunType type;

        public int Cost { get; set; }

        public enum ShootType { Normal, MachineGun, Big, Steady};
        public enum GunType { Glock, Shotgun, WormGun}

        private void Start()
        {
            bullet.GetComponent<BulletSpeedController>().Speed = bulletSpeed;
            bullet.GetComponent<DamageController>().BaseDamage = baseDamage;
            bullet.GetComponent<LifeSpanController>().Lifespan = bulletLifeSpan;
            
            switch(this.gameObject.name)
            {
                case "Glock":
                    Cost = 1;
                    break;
                case "WormGun":
                    Cost = 3;
                    break;
                case "superShotgun":
                    Cost = 10;
                    break;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            GameObject.Find("BuyText").GetComponent<Text>().color = new Color(0, 0, 0, 255);
        }
        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player" && (other.GetComponent<PlayerController>().heldGun.GetComponent<Gun>().type != this.type) && Input.GetKey(KeyCode.F))
            {
                if (MoneyController.CanBuy(this))
                    other.GetComponent<PlayerController>().PickupGun(this.gameObject);
            }
        }
        void OnTriggerExit(Collider other)
        {
            GameObject.Find("BuyText").GetComponent<Text>().color = new Color(0, 0, 0, 0);
        }
    }
}
