using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{
    public WeaponScript StartingGun;
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
            weapon.transform.parent = gameObject.transform;    
            weapon.GetComponent<BoxCollider>().enabled = false;
            weapon.enabled = true;  
        }
    }
    public GameObject hat;

    public Transform gunBone;
    public Transform hatBone;


    void Start() {
        hat = Instantiate(hat);
        Weapon = Instantiate(StartingGun);
    }

    void Update() {
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

    public void ReturnToDefaultGun()
    {
        Destroy(Weapon.gameObject);
        Weapon = Instantiate(StartingGun);
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
