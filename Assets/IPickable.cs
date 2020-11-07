using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable: IInteractable
{
    void Pickup(GearController controller);
}
