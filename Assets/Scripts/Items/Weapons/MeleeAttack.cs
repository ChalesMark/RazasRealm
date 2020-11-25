using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackable
{

    // Start is called before the first frame update
    public void RunWeaponComponent(PlayerController controller) 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
            Attack(controller);
    }


    public void Attack(PlayerController controller)
    {
        controller.animator.SetTrigger("slash");
    }

}
