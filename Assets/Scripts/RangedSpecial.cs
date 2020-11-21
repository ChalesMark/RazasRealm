using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpecial : MonoBehaviour, ISpecialAttack
{ 


    public void RunWeaponComponent(Animator animator) 
    {
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            InvokeSpecial(animator);
        }
    }
    

    public void InvokeSpecial(Animator animator) 
    {
        print("idfk lmfao");
    }
}
