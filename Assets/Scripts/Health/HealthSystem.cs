using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 6;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int amount)
    {
        if (currentHealth - amount >= 0)
            currentHealth -= amount;
        else
        {
            currentHealth = 0;
        }
    }

    public void RecoverHealth(int amount)
    {
        if(currentHealth + amount <= maxHealth)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth = maxHealth;
        }

    }
}
