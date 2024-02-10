using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    // Variables to track debug statuses
    private bool godModeEnabled = false;
    private bool infiniteBulletsEnabled = false;
    private bool noClippingEnabled = false;
    private bool allItemsEnabled = false;
    private bool skipCutscenesEnabled = false;

    // Reference to the debug menu UI panel
    public GameObject debugMenu;

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
        // Toggle the active state of the debug menu
        debugMenu.SetActive(!debugMenu.activeSelf);

        // If the debug menu is now active, lock the cursor
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
        // Implement God Mode activation logic here
        Debug.Log("God Mode " + (godModeEnabled ? "Enabled" : "Disabled"));
    }

    // Toggle Infinite Bullets
    public void ToggleInfiniteBullets()
    {
        infiniteBulletsEnabled = !infiniteBulletsEnabled;
        // Implement infinite bullets activation logic here
        Debug.Log("Infinite Bullets " + (infiniteBulletsEnabled ? "Enabled" : "Disabled"));
    }

    // Toggle No Clipping
    public void ToggleNoClipping()
    {
        noClippingEnabled = !noClippingEnabled;
        // Implement no clipping activation logic here
        Debug.Log("No Clipping " + (noClippingEnabled ? "Enabled" : "Disabled"));
    }

    // Give All Items
    public void GiveAllItems()
    {
        allItemsEnabled = true;
        // Implement logic to give all items here
        Debug.Log("All Items Given");
    }

    // Skip Cutscenes
    public void SkipCutscenes()
    {
        skipCutscenesEnabled = true;
        // Implement logic to skip cutscenes here
        Debug.Log("Cutscenes Skipped");
    }
}