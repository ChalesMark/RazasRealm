using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public IWeaponComponent[] weaponComponents;


    private void Start() {
        weaponComponents = GetComponents<IWeaponComponent>();
    }
    // Update is called once per frame
    void Update()
    {
        foreach(IWeaponComponent component in weaponComponents) {
            component.RunWeaponComponent();
        }
    }
}
