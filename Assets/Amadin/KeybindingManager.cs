using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class KeybindingManager : MonoBehaviour
{
    public Button[] keybindingButtons;
    public GameObject keybindingPopup;
    public TextMeshProUGUI keybindingText;

    private Key[] defaultKeybindings;
    private bool rebinding;
    private int rebindingIndex;

    void Start()
    {
        LoadKeybindings();
        UpdateKeybindingButtons();
    }

    void Update()
    {
        if (rebinding && Keyboard.current.anyKey.isPressed)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CancelRebinding();
            }
            else
            {
                foreach (Key key in System.Enum.GetValues(typeof(Key)))
                {
                    if (Keyboard.current[key].wasPressedThisFrame)
                    {
                        SetNewKeybinding(rebindingIndex, key);
                        break;
                    }
                }
            }
        }
    }

    public void StartRebinding(int index)
    {
        rebinding = true;
        rebindingIndex = index;
        keybindingPopup.SetActive(true);
        keybindingText.text = "Press button to rebind";
    }

    public void CancelRebinding()
    {
        rebinding = false;
        keybindingPopup.SetActive(false);
    }

    public void SetNewKeybindingOnClick(int index)
    {
        StartRebinding(index);
    }

    private void SetNewKeybinding(int index, Key key)
    {
        if (IsKeybindingConflict(key))
        {
            Debug.LogWarning("Keybinding conflict detected. Please choose another key.");
            // Provide visual feedback to the user
            return;
        }

        defaultKeybindings[index] = key;
        PlayerPrefs.SetInt("Keybinding_" + index, (int)key);
        PlayerPrefs.Save();

        UpdateKeybindingButtons();
        CancelRebinding();
    }

    private void UpdateKeybindingButtons()
    {
        for (int i = 0; i < keybindingButtons.Length; i++)
        {
            keybindingButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = defaultKeybindings[i].ToString();
        }
    }

    private void LoadKeybindings()
    {
        defaultKeybindings = new Key[keybindingButtons.Length];
        for (int i = 0; i < keybindingButtons.Length; i++)
        {
            defaultKeybindings[i] = (Key)PlayerPrefs.GetInt("Keybinding_" + i, (int)Key.None);
        }
    }

    private bool IsKeybindingConflict(Key key)
    {
        foreach (Key defaultKey in defaultKeybindings)
        {
            if (defaultKey == key)
                return true;
        }
        return false;
    }
}
