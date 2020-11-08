using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpecial : MonoBehaviour, ISpecialAttack
{ 


    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            InvokeSpecial();
        }
    }
    

    public void InvokeSpecial() 
    {
        print("idfk lmfao");
    }
}
