using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControls : MonoBehaviour
{
    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode meleeKey = KeyCode.Mouse1;
    public KeyCode actionKey = KeyCode.F;


    public KeyCode GetActionKey() {
        return actionKey;
    }
}
