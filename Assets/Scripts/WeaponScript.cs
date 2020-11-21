using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public IWeaponComponent[] weaponComponents;
    public Animator PlayerAnimator{get;set;}

    private void OnEnable() {
        weaponComponents = GetComponents<IWeaponComponent>();
        PlayerAnimator = GetComponentInParent<Animator>();
        print(PlayerAnimator);
    }

    // Update is called once per frame
    void Update()
    {   
            foreach(IWeaponComponent component in weaponComponents) 
            {
                component.RunWeaponComponent(PlayerAnimator);
            }
    }
}
