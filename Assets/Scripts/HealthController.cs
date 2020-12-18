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
    private WaveManager waveManager;

    private Slider healthBar;
    private Text healthBarText;

    private void Start()
    {
        if (GameObject.Find("WaveManager"))
            waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GetComponent<AudioSource>() != null)
            audio = GetComponent<AudioSource>();

        currHealth = maxHealth;

        if (gameObject.tag == "Enemy")
        {
            if(waveManager)
                maxHealth += enemyHealthScaling * waveManager.GetCurrentWave();
            currHealth = maxHealth;
            if(GetComponent<BossController>() != null)
            {
                healthBar = Camera.main.transform.Find("Canvas").Find("BossHealth").GetComponent<Slider>();
                healthBarText = Camera.main.transform.Find("Canvas").Find("BossHealth").Find("HealthText").GetComponent<Text>();
                healthBar.maxValue = maxHealth;
                healthBar.value = currHealth;
                healthBarText.text = maxHealth + " / " + maxHealth;
                healthBar.gameObject.SetActive(true);
            }
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
        if (healthBarText != null)
            healthBarText.text = currHealth + " / " + maxHealth;
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
            if (GetComponent<BossController>() != null)
            {
                healthBar.gameObject.SetActive(false);
                GetComponent<BossController>().DropLoot();
            }
            else
                GetComponent<BaddieController>().DropLoot();
        }
        else if (tag == "Player") 
        {
           GetComponent<MoneyController>().SetMoney(0);
           Camera.main.transform.Find("Canvas").Find("BossHealth").gameObject.SetActive(false);
           gameManager.StartGame();
        }
        Destroy(gameObject);
    }
}
