using UnityEngine;
using UnityEngine.UI;

public class CheatMenu : MonoBehaviour
{
    // Variables to track debug statuses
    private bool godModeEnabled = false;
    private bool infiniteBulletsEnabled = false;
    private bool noClippingEnabled = false;
    private bool skipCutscenesEnabled = false;
    private bool allItemsEnabled;

    // References
    public GameObject debugMenu;
    public PlayerHealth playerHealth;
    public InventoryManager inventoryManager;
    public GunController gunController;

    private void Start()
    {
        // Hide the debug menu initially
        debugMenu.SetActive(false);
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
            EnableNoClipping();
        }
        else
        {
            DisableNoClipping();
        }
        
        Debug.Log("No Clipping " + (noClippingEnabled ? "Enabled" : "Disabled"));
    }

    private void EnableNoClipping()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    private void DisableNoClipping()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    // Give All Items
    public void GiveAllItems()
    {
        allItemsEnabled = true;
        inventoryManager.GiveAllItemsToPlayer();
        Debug.Log("All Items Given");
    }
}