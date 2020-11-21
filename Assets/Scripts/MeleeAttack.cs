using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackable
{

    // Start is called before the first frame update
    public void RunWeaponComponent(Animator animator) 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 

            Attack(animator);
    }


    public void Attack(Animator animator)
    {
        animator.SetTrigger("slash");
    }

}
