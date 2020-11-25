using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable: IWeaponComponent
{
    void Attack(PlayerController controller);
} 
