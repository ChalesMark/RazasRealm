using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackable
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private int projectileCount;
    [SerializeField]
    private int spread;
    [SerializeField]
    private int fireRate;
    [SerializeField]
    private bool automatic;



    public void RunWeaponComponent() 
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            Attack();
        }
    }


    public void Attack()
    {
        print("BLAM");
        //Instantiate(projectile);
    }
}
