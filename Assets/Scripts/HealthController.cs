using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    // Start is called before the first frame update
    public int maxHealth;
    public int enemyHealthScaling;
    //[HideInInspector]
    public  int currHealth;
    [HideInInspector]
    public bool dead = false;
    public AudioClip deathSound;
    public AudioClip damageSound;
    private AudioSource audio;
    private bool invincible = false;
    private GameManager gameManager;


    private Slider healthBar;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(GetComponent<AudioSource>() != null)
            audio = GetComponent<AudioSource>();

        currHealth = maxHealth;
        if (gameObject.tag == "Enemy")
        {
            maxHealth += enemyHealthScaling * GameObject.Find("WaveManager").GetComponent<WaveManager>().GetCurrentWave();
        }
        else if (gameObject.tag == "Player")
        {
            healthBar = Camera.main.transform.Find("Canvas").Find("PlayerHealth").GetComponent<Slider>();
            healthBar.maxValue = maxHealth;
            healthBar.value = currHealth;
        }
    }

    private void Update()
    {
        if(healthBar != null)
            healthBar.value = currHealth;
    }


    public void DecreaseCurrentHealth(int damage) {
        currHealth -= damage;
        if (damageSound)
            audio.PlayOneShot(damageSound);
        if(currHealth <= 0 && !dead) {
            currHealth = 0;
            Kill();
        }
    }

    public void Heal(int health) {
        currHealth += health;
        if (currHealth >= maxHealth)
            currHealth = maxHealth;
    }


    public void Kill() {
        dead = true;

        if (deathSound)
            GameObject.Find("GameManager").GetComponent<AudioSource>().PlayOneShot(deathSound);
        if(tag == "Enemy")
        {
            gameManager.GetPlayerObject().GetComponent<MoneyController>().AddMoney(GetComponent<IEnemyController>().KillGold);
            GetComponent<BaddieController>().DropLoot();
        }
        else if (tag == "Player") 
        {
           gameManager.StartGame();
        }
        if (GetComponent<BossController>() != null)
        {
            GetComponent<BossController>().Die();
        }
        Destroy(gameObject);
    }
}
