using UnityEngine;
using TMPro;

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

    // Teleporters
    public Transform[] teleporters;
    public TMP_Dropdown teleportDropdown;

    private CameraController cameraController; // Reference to the CameraController script

    private void Start()
    {
        // Hide the debug menu initially
        debugMenu.SetActive(false);

        // Find and store a reference to the CameraController script
        cameraController = FindObjectOfType<CameraController>();

        // Set up teleport dropdown
        if (teleportDropdown != null)
        {
            teleportDropdown.ClearOptions();

            // Populate dropdown with teleporter names
            foreach (Transform teleporter in teleporters)
            {
                teleportDropdown.options.Add(new TMP_Dropdown.OptionData(teleporter.gameObject.name));
            }

            // Add listener for dropdown value change
            teleportDropdown.onValueChanged.AddListener(TeleportDropdownValueChanged);
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

    // Teleport using dropdown selection
    private void TeleportDropdownValueChanged(int index)
    {
        if (index >= 0 && index < teleporters.Length)
        {
            Transform teleporter = teleporters[index];
            if (teleporter != null)
            {
                playerTransform.position = teleporter.position;
                Debug.Log("Teleported using dropdown to: " + teleporter.gameObject.name);

                // Reset camera after teleporting
                if (cameraController != null)
                {
                    cameraController.ResetCamera();
                }
            }
            else
            {
                Debug.LogWarning("Teleporter not found for dropdown index: " + index);
            }
        }
    }
}

