using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    private bool isPlayerTurn = true;
    private bool combatEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        // Begin combat with the player's turn
        isPlayerTurn = true;
        combatEnded = false;
        Debug.Log("Combat begins! Player's turn.");
    }

    private void Update()
    {
        if (!combatEnded)
        {
            if (isPlayerTurn)
            {
                // Player's turn
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Playrer attack
                    int playerDamage = UnityEngine.Random.Range(10, 20);
                    Debug.Log("Player attacks for " + playerDamage + " damage!");
                    enemy.GetComponent<Health>().TakeDamage(playerDamage);

                    isPlayerTurn = false;
                }
            }
            else
            {
                // Enemy's turn
                int enemyDamage = UnityEngine.Random.Range(5, 15);
                Debug.Log("Enemy attacks for " + enemyDamage + " damage!");
                player.GetComponent<Health>().TakeDamage(enemyDamage);

                isPlayerTurn = true;
            }

            // Check if combat has ended
            if (player.GetComponent<Health>().IsDead())
            {
                Debug.Log("Player is defeated. Enemy wins!");
                combatEnded = true;
            }
            else if (enemy.GetComponent<Health>().IsDead())
            {
                Debug.Log("Enemy is defeated. Player wins!");
                combatEnded = true;
            }
        }
    }
}

// Example Health script
public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
