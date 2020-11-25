using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangedAttack))]
public class WeaponAim : MonoBehaviour, IWeaponComponent
{ 

    [SerializeField]
    private float targetFOV = 50;

    private float zoomSpeed = 150f;

    [SerializeField]
    private int slowSpeed = 7;

    private float startingFov = 60.0f;

    private int damageBuff;

    [SerializeField]
    private float percentageBoost = 20f;

    private RangedAttack rangedAttack;

    void Awake() {
        rangedAttack = GetComponent<RangedAttack>();
        damageBuff = (int)(rangedAttack.baseDamage * percentageBoost/100);
    }


    public void RunWeaponComponent(PlayerController controller) 
    {
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            controller.DecreaseSpeed(slowSpeed);
            rangedAttack.baseDamage += damageBuff;
        }
        if(Input.GetKeyUp(KeyCode.Mouse1)) {
            controller.IncreaseSpeed(slowSpeed);
            rangedAttack.baseDamage -= damageBuff;
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            if(Camera.main.fieldOfView > targetFOV) 
            {
                Camera.main.fieldOfView -= zoomSpeed * Time.deltaTime;
            }
            return;
        }

        if(Camera.main.fieldOfView < startingFov) {
            Camera.main.fieldOfView += zoomSpeed * Time.deltaTime;
        }
    }

}
