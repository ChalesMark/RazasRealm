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
    private bool invincible = false;


    private Slider healthBar;

    private void Start()
    {
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
        healthBar.value = currHealth;
    }


    public void DecreaseCurrentHealth(int damage) {
        currHealth -= damage;
        if(currHealth <= 0 && !dead) {
            currHealth = 0;
            Kill();
        }
    }

    public void Heal(int health) {
        int amountHealed = health + currHealth;
        if(amountHealed > maxHealth) {
            currHealth = maxHealth;
            return;
        }
        currHealth += amountHealed;
    }


    public void Kill() {
        dead = true;
        if(tag == "Enemy")
            GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayerObject().GetComponent<MoneyController>().AddMoney(GetComponent<IEnemyController>().KillGold);
        if (GetComponent<BossController>() != null)
        {
            GetComponent<BossController>().Die();
        }
        Destroy(gameObject);
    }
}
