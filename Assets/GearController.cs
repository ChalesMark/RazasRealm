using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{
    public GameObject weapon;
    public GameObject hat;

    public Transform gunBone;
    public Transform hatBone;


    void Start() {
        weapon = Instantiate(weapon);
        hat = Instantiate(hat);

        foreach(Component component in weapon.GetComponents(typeof(IWeaponComponent))) {
            Behaviour bhvr = (Behaviour)component;
            bhvr.enabled = true;
        }
    }

    void Update() {

        if(weapon.transform.parent == null) {
            weapon.transform.parent = gameObject.transform;
            weapon.GetComponent<BoxCollider>().enabled = false;
        }

        if(hat.transform.parent == null) {
            hat.transform.parent = gameObject.transform;
        }

        weapon.transform.position = gunBone.position;
        weapon.transform.rotation = gunBone.rotation;

        hat.transform.position = hatBone.position;
        hat.transform.rotation = hatBone.rotation;
    }


    public void SwitchWeapon(GameObject newWeapon) {
        Destroy(weapon);
        weapon = newWeapon;
        foreach(Component component in newWeapon.GetComponents(typeof(IWeaponComponent))) {
            Behaviour bhvr = (Behaviour)component;
            bhvr.enabled = true;
            print("ENABLING " + bhvr);
        }
    }

    public void SwitchHat(GameObject newHat) {
        Destroy(hat);
        hat = newHat;
    }

    private void RemoveSafety(GameObject weapon)
    {

    }
}
