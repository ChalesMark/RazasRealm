using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool open;
    bool lastState;
    Animator Animator;
    BoxCollider boxCollider;
    void Start()
    {
        open = false;
        lastState = false;
        Animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastState != open)
        {
            lastState = open;
            Animator.SetBool("open", lastState);
            boxCollider.enabled = !open;
        }
    }
}
