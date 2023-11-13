using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDown : MonoBehaviour
{
    public Transform playerCharacter;
    public Transform enemyCharacter;

    public float duelDuration = 3f;
    public float reactionTime = 0.5f;

    private bool isDueling = false;
    private bool playerReacted = false;
    private float currentTime = 0f;

    private void Update()
    {
        if (isDueling)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= duelDuration)
            {
                EndDuel();
            }

            if (Input.GetKeyDown(KeyCode.O) && !playerReacted)
            {
                PlayerReacted();
            }
        }
    }

    public void StartDuel()
    {
        isDueling = true;
        playerReacted = false;
        currentTime = 0f;

        // Position characters appropriately for the showdown
        // Might want to animate them into position
        // Start a countdown or animation to signal the start of the duel

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);

        // Start of the duel here 
        // Wait for the reaction time before allowing the player to react
        yield return new WaitForSeconds(reactionTime);
        playerReacted = true;
    }

    private void PlayerReacted()
    {
        // Handle the player's successful reaction
        Debug.Log("Player Reacted!");
        EndDuel();
    }

    private void EndDuel()
    {
        isDueling = false;

        // Handle the end of the duel 
        Debug.Log("Duel Ended!");

        // You can reset character positions and prepare for the next showdown
    }

    // You can call StartDuel() when you want to start a showdown
}