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
    private float autofireRate;
    [SerializeField]
    private bool automatic;
    [SerializeField]
    private float manualfireCooldown;
    [SerializeField]
    private int baseDamage;
    [SerializeField]
    private int projectileSpeed;
    [SerializeField]
    private float projectileLifeSpan;
    [SerializeField]
    private ShootType shootType;

    private Animator animator;

    private bool onCooldown = false;

    public enum ShootType { Normal, MachineGun, Steady, Big};

    public void RunWeaponComponent() 
    {
        //MIGHT WANT TO MOVE THIS INTO WEAPONSCRIPT
        if(animator == null) {
            animator = GetComponentInParent<Animator>();
        }

        if (onCooldown)
            return;
        else if (automatic && Input.GetKey(KeyCode.Mouse0))
            Attack();
        else if (Input.GetKeyDown(KeyCode.Mouse0))
            Attack();
        //for testing
        else if (Input.GetKeyDown(KeyCode.Q))
            projectileCount++;
        else if (Input.GetKeyDown(KeyCode.E))
            projectileCount--;

    }


    public void Attack()
    {
        animator.SetInteger("shootType", (int)System.Enum.Parse(typeof(ShootType), shootType.ToString()));
        animator.SetTrigger("shoot");
        projectile.GetComponent<DamageController>().BaseDamage = baseDamage;
        projectile.GetComponent<BulletSpeedController>().Speed = projectileSpeed;
        projectile.GetComponent<LifeSpanController>().Lifespan = projectileLifeSpan;
        for (int i = 0; i < projectileCount; i++)
            Instantiate(projectile, gameObject.transform.Find("projectileSpawn").transform.position, transform.rotation * Quaternion.AngleAxis(UnityEngine.Random.Range(-spread, spread), Vector3.up));

        onCooldown = true;
        StartCoroutine(CooldownCoroutine(automatic ? autofireRate : manualfireCooldown));
        print(gameObject.name + " shot a " + projectile.name + "\nDamage: " + baseDamage + "\nSpeed: " + projectileSpeed + "\nLifespan: " + projectileLifeSpan);
    }

    private IEnumerator CooldownCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onCooldown = false;
    }

}
