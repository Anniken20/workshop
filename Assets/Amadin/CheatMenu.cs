using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StarterAssets;

public class CheatMenu : MonoBehaviour
{
    // Variables to track debug statuses
    private bool godModeEnabled = false;
    private bool infiniteBulletsEnabled = false;
    private bool noClippingEnabled = false;
    private bool allItemsEnabled;

    // References
    public GameObject debugMenu;
    public PlayerHealth playerHealth;
    public InventoryManager inventoryManager;
    public GunController gunController;
    public FreeCamController freeCamController;
    public Transform playerTransform;

    // Our Checkpoints and teleporters
    public Transform[] checkpoints;
    public Transform[] teleporters;

    // Text for the teleports
    public Text[] checkpointTexts;
    public Text[] teleporterTexts;

    private void Start()
    {
        // Hide the debug menu initially
        debugMenu.SetActive(false);

        // Assign teleportation functions to text UI elements
        for (int i = 0; i < checkpointTexts.Length; i++)
        {
            int index = i; 
            checkpointTexts[i].GetComponent<Button>().onClick.AddListener(() => TeleportToCheckpoint(index));
        }

        for (int i = 0; i < teleporterTexts.Length; i++)
        {
            int index = i; 
            teleporterTexts[i].GetComponent<Button>().onClick.AddListener(() => TeleportToTeleporter(index));
        }
    }

    private void Update()
    {
        // Toggle debug menu visibility when the ` key is pressed
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleCheatMenu();
        }
    }

    // Toggle debug menu visibility
    private void ToggleCheatMenu()
    {
        debugMenu.SetActive(!debugMenu.activeSelf);

        if (debugMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Toggle God Mode
    public void ToggleGodMode()
    {
        godModeEnabled = !godModeEnabled;
        if (godModeEnabled)
        {
            playerHealth.ToggleGodMode();
        }
        else
        {
            playerHealth.godModeEnabled = false;
        }
        Debug.Log("God mode" + (godModeEnabled ? "Enabled" : "Disabled"));
    }

    // Toggle Infinite Bullets
    public void ToggleInfiniteBullets()
    {
        infiniteBulletsEnabled = !infiniteBulletsEnabled;
        
        if (infiniteBulletsEnabled)
        {
            // Enable infinite bullets logic in GunController
            gunController.EnableInfiniteBullets();
        }
        else
        {
            // Disable infinite bullets logic in GunController
            gunController.DisableInfiniteBullets();
        }
        
        Debug.Log("Infinite Bullets " + (infiniteBulletsEnabled ? "Enabled" : "Disabled"));
    }


    // Toggle No Clipping
    public void ToggleNoClipping()
    {
        noClippingEnabled = !noClippingEnabled;
        if (noClippingEnabled)
        {
            freeCamController.enabled = true;

        }
        else
        {
            freeCamController.enabled = false;
        }

        Debug.Log("No Clipping " + (noClippingEnabled ? "Enabled" : "Disabled"));
    }

    // Give All Items
    public void GiveAllItems()
    {
        allItemsEnabled = true;
        inventoryManager.GiveAllItemsToPlayer();
        Debug.Log("All Items Given");
    }

    // Teleport to checkpoint by index
    private void TeleportToCheckpoint(int index)
    {
        if (index >= 0 && index < checkpoints.Length)
        {
            playerTransform.position = checkpoints[index].position;
            Debug.Log("Teleported to checkpoint " + index);
        }
    }

    // Teleport to teleporter by index
    private void TeleportToTeleporter(int index)
    {
        if (index >= 0 && index < teleporters.Length)
        {
            playerTransform.position = teleporters[index].position;
            Debug.Log("Teleported to teleporter " + index);
        }
    }
}