using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    private GunController gunController;
    private PlayerHealth healthScript;
    private InventoryManager inventoryManager;

    public Button toggleUnlimitedAmmoButton;
    public Button toggleGodModeButton;
    public Button giveAllItemsButton;
    public Button toggleNoclipButton;
    public Button skipCutsceneButton;

    public GameObject menuPanel;

    private bool menuEnabled = false;
    private bool isNoclipEnabled = false;

    private void Start()
    {
        gunController = GetComponent<GunController>();
        healthScript = GetComponent<PlayerHealth>();
        inventoryManager = InventoryManager.Instance;

        // Add listeners to the buttons
        toggleUnlimitedAmmoButton.onClick.AddListener(ToggleUnlimitedAmmo);
        toggleGodModeButton.onClick.AddListener(ToggleGodMode);
        giveAllItemsButton.onClick.AddListener(GiveAllItems);
        toggleNoclipButton.onClick.AddListener(ToggleNoclip);
        skipCutsceneButton.onClick.AddListener(SkipCutscene);

        HideMenu();
    }

    private void Update()
    {
        // Toggle menu when BackQuote key is pressed
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            ToggleMenu();
        }
    }

    private void ToggleUnlimitedAmmo()
    {
        // Toggle unlimited ammo
        gunController.ToggleUnlimitedAmmo();
        HideMenu();
    }

    private void ToggleGodMode()
    {
        // Toggle god mode
        healthScript.ToggleGodMode();
        HideMenu();
    }

    private void GiveAllItems()
    {
        // Give all items
        inventoryManager.GiveAllItems();
        HideMenu();
    }

    private void ToggleNoclip()
    {
        // Toggle noclip mode
        isNoclipEnabled = !isNoclipEnabled;
        GetComponent<Collider>().enabled = !isNoclipEnabled;
        Time.timeScale = isNoclipEnabled ? 0f : 1f;
        HideMenu();
    }

    private void SkipCutscene()
    {
        // Skip current cutscene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ToggleMenu()
    {
        // Toggle visibility of the menu
        menuEnabled = !menuEnabled;
        menuPanel.SetActive(menuEnabled);
    }

    private void HideMenu()
    {
        // Hide the menu
        menuEnabled = false;
        menuPanel.SetActive(false);
    }
}
