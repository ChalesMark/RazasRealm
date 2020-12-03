using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackable
{
    public string gunName;

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
    private bool loopingAudio;
    [SerializeField]
    private float manualfireCooldown;
    [SerializeField]
    private int projectileSpeed;
    [SerializeField]
    private float projectileLifeSpan;
    [SerializeField]
    private ShootType shootType;
    [SerializeField]
    private int ammoLeft;
    [SerializeField]
    public int maxAmmo;

    public int baseDamage;

    CameraController cameraController;

    private AudioSource soundEffect;

    private bool onCooldown = false;

    public enum ShootType { Normal, MachineGun, Steady, Big};

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        ammoLeft = maxAmmo;
        cameraController.SetAmmoText(ammoLeft, maxAmmo);
        if (GetComponent<AudioSource>() != null)
            soundEffect = GetComponent<AudioSource>();
    }

    public void RunWeaponComponent(PlayerController controller) 
    {
        if (onCooldown)
            return;
        else if (automatic && Input.GetKey(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (maxAmmo == 0)
            {
                cameraController.SetAmmoText(ammoLeft, maxAmmo);
                Attack(controller);
            }
            else if (ammoLeft > 0)
            {
                ammoLeft--;
                cameraController.SetAmmoText(ammoLeft, maxAmmo);
                Attack(controller);
                if (ammoLeft <= 0)
                {
                    if (!loopingAudio)
                        GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(soundEffect.clip, 0.75f);
                    controller.GetComponent<GearController>().ReturnToDefaultGun();
                }
            }
        }
        else if(loopingAudio && soundEffect.isPlaying && !Input.GetKey(KeyCode.Mouse0))
        {
            soundEffect.Stop();
        }
    }

    public bool ShouldPickup()
    {
        if (maxAmmo == 0 || ammoLeft == maxAmmo)
            return false;
        return true;
    }

    internal void GetAmmo(int value)
    {
        ammoLeft += value;
        if (ammoLeft > maxAmmo)
            ammoLeft = maxAmmo;
        cameraController.SetAmmoText(ammoLeft, maxAmmo);
    }

    public void Attack(PlayerController controller)
    {

        controller.animator.SetInteger("shootType", (int)System.Enum.Parse(typeof(ShootType), shootType.ToString()));
        controller.animator.SetTrigger("shoot");
        
        projectile.GetComponent<DamageController>().BaseDamage = baseDamage;
        projectile.GetComponent<BulletSpeedController>().Speed = projectileSpeed;
        projectile.GetComponent<LifeSpanController>().Lifespan = projectileLifeSpan;

        Quaternion leveledForward = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y, 0);

        for (int i = 0; i < projectileCount; i++)
            Instantiate(projectile, gameObject.transform.Find("projectileSpawn").transform.position, leveledForward * Quaternion.AngleAxis(UnityEngine.Random.Range(-spread, spread), Vector3.up));

        PlaySoundEffect();

        onCooldown = true;
        StartCoroutine(CooldownCoroutine(automatic ? autofireRate : manualfireCooldown));
    }

    private IEnumerator CooldownCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onCooldown = false;
    }

    private void PlaySoundEffect()
    {
        if (loopingAudio)
        {
            if (!soundEffect.isPlaying)
            {
                soundEffect.loop = true;
                soundEffect.Play();
            }
        }
        else
            soundEffect.Play();
    }
}
