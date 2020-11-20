using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{
    public WeaponScript weapon;
    
    private WeaponScript Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
            weapon.enabled = true;      
        }
    }

    public GameObject hat;

    public Transform gunBone;
    public Transform hatBone;


    void Start() {
        hat = Instantiate(hat);
        Weapon = Instantiate(weapon);
    }

    void Update() {

        if(weapon.transform.parent == null) {
            weapon.transform.parent = gameObject.transform;
        }

        /*
        if(hat.transform.parent == null) {
            hat.transform.parent = gameObject.transform;
        }
        */

        weapon.transform.position = gunBone.position;
        weapon.transform.rotation = gunBone.rotation;

        hat.transform.position = hatBone.position;
        hat.transform.rotation = hatBone.rotation;
    }


    public void SwitchWeapon(WeaponScript newWeapon) {
        Destroy(Weapon.gameObject);
        Weapon = newWeapon;
    }

    public void SwitchHat(GameObject newHat) {
        Destroy(hat.gameObject);
        hat = Instantiate(newHat);
    }

    internal void RecieveValues(GameObject lastHat, GameObject lastGun)
    {
        SwitchWeapon(lastGun.GetComponent<WeaponScript>());
        SwitchHat(lastHat);
        Destroy(lastHat.gameObject);
        Destroy(lastGun.gameObject);
    }
}
