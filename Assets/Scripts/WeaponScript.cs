using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public IWeaponComponent[] weaponComponents;
    public bool canShoot;           // Used so the player cant shoot during menus

    private void Start() {
        canShoot = true;
        weaponComponents = GetComponents<IWeaponComponent>();
    }
    // Update is called once per frame
    void Update()
    {
        if (canShoot)
            foreach(IWeaponComponent component in weaponComponents) {
                component.RunWeaponComponent();
            }
    }
}
