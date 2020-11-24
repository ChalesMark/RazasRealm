using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAndBob : MonoBehaviour
{
    public int spinTime,bobTime;
    public float heightMod,heightDivide;


    float spin = 0;
    float bob = 0;
    float originalHeight;

    void Start()
    {
        if (spinTime == 0)
            spinTime = 100;
        if (bobTime == 0)
            bobTime = 2;

        spin = Random.Range(0, 100);
        bob = Random.Range(0, 100);

        originalHeight = transform.position.y+ heightMod;
    }

    void Update()
    {
        if (spin == float.MaxValue)
            spin = 0;
        if (bob == float.MaxValue)
            bob = 0;

        spin += spinTime * Time.deltaTime;
        bob += bobTime * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(0, spin, 0);
        this.transform.position = new Vector3(this.transform.position.x, originalHeight + Mathf.Cos(bob)/ heightDivide, this.transform.position.z);
    }
}
