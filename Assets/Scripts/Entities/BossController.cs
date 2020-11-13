using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public Slider healthBar;
    public GameObject dropItem;
    private HealthController healthController;
    private IEnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        healthController = GetComponent<HealthController>();
        enemyController = GetComponent<IEnemyController>();

        healthBar.maxValue = healthController.maxHealth;
        healthBar.value = healthController.currHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyController.PlayerSpotted)
        {
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }

        healthBar.value = healthController.currHealth;
    }

    public void Die()
    {
        healthBar.gameObject.SetActive(false);
        Instantiate(dropItem, transform.position, transform.rotation);
    }
}
