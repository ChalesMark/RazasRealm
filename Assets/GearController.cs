using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{
    public GameObject gun;
    public GameObject hat;
    public Transform gunBone;
    public Transform hatBone;


    void Start() {
        gun = Instantiate(gun);
        hat = Instantiate(hat);
        gun.name = "Starting Gun";
        hat.name = "Starting Hat";
    }

    void Update() {

        if(gun.transform.parent == null) {
            gun.transform.parent = gameObject.transform;
            gun.GetComponent<BoxCollider>().enabled = false;
        }

        if(hat.transform.parent == null) {
            hat.transform.parent = gameObject.transform;
        }

        gun.transform.position = gunBone.position;
        gun.transform.rotation = gunBone.rotation;

        hat.transform.position = hatBone.position;
        hat.transform.rotation = hatBone.rotation;
    }


    public void SwitchGun(GameObject newGun) {
        Destroy(gun);
        gun = newGun;
    }

    public void SwitchHat(GameObject newHat) {
        Destroy(hat);
        hat = newHat;
    }
}
