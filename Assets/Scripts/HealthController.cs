using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    // Start is called before the first frame update
    public int maxHealth;
    public int currHealth;
    public int enemyHealthScaling;
    bool dead = false;

    private void Start()
    {
        if(gameObject.tag == "Enemy")
        {
            maxHealth += enemyHealthScaling * GameObject.Find("WaveManager").GetComponent<WaveManager>().GetCurrentWave();
        }
        currHealth = maxHealth;
    }


    public void DecreaseCurrentHealth(int damage) {
        currHealth -= damage;
        if(currHealth < 0 && !dead) {
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
