using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpanController : MonoBehaviour
{
    public float Lifespan;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(Lifespan);
        Destroy(gameObject);
    }

    public void SetLifespan(float Lifespan) {
        this.Lifespan = Lifespan;
    }
}
