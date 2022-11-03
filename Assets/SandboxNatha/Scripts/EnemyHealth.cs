using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    public float health;
    public float maxHealth;

    public GameObject healthBarUI;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }


    private void updateHealth(float damage)
    {
        health -= damage;

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        slider.value = Mathf.Clamp(health / maxHealth, 0, 1f);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            float damage=other.GetComponent<WeaponProperties>().damage;
            updateHealth(damage);
        }
    }
}
