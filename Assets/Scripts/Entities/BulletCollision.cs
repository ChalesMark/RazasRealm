using UnityEngine.Events;
using UnityEngine;
using System;
 
public class BulletCollision : MonoBehaviour
{
    public event EventHandler<BulletCollisionEventArgs> OnBulletCollision;

    private void OnTriggerEnter(Collider other)
    {      
        OnBulletCollision?.Invoke(this, new BulletCollisionEventArgs(other.transform, 25, 2));       
    }
}