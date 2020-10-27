using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAndBob : MonoBehaviour
{
    float spin = 0;
    float bob = 0;

    // Update is called once per frame
    void Update()
    {
        spin += 100*Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(0, spin, 0);
    }
}
