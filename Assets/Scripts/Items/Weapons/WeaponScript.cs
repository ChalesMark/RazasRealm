using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public IWeaponComponent[] weaponComponents;
    public PlayerController playerController;

    private void OnEnable() {
        weaponComponents = GetComponents<IWeaponComponent>();
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {   
            foreach(IWeaponComponent component in weaponComponents) 
            {
                component.RunWeaponComponent(playerController);
            }
    }
}
