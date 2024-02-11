using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    //Our Inventory items
    public List<AllItems> inventoryItems = new List<AllItems>();

    [SerializeField] GameObject[] itemUIImages;

    private void Awake()
    {
        Instance = this;
    }

    //Add items to inventory
    public void AddItem(AllItems item)
    {
        if(!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
        }
    }

    //Remove items to inventory
    public void RemoveItem(AllItems item)
    {
        if(inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
        }
    }

    public void UpdateCollectedItemUI()
    {
        foreach (AllItems item in inventoryItems)
        {
            // Enable the UI image corresponding to the collected item
            int itemIndex = (int)item;
            if (itemIndex >= 0 && itemIndex < itemUIImages.Length)
            {
                itemUIImages[itemIndex].SetActive(true);
                StartCoroutine(HideUIAfterDelay(itemUIImages[itemIndex]));
                
            }
        }
    }

    // DEBUG
    public void GiveAllItemsToPlayer()
    {
        foreach (AllItems item in System.Enum.GetValues(typeof(AllItems)))
        {
            AddItem(item);
        }
    }
    
    private IEnumerator HideUIAfterDelay(GameObject uiElement)
    {
        yield return new WaitForSeconds(60f); // Wait for 60 seconds (1 minute)
        uiElement.SetActive(false); // Hide the UI element after 1 minute
    }

    //All Available Inventory Items in game
    public enum AllItems
    {
        SugarCubes,
        Whiskey,
        Badge,
        Reliquary,
        GuestBook,
        TrunkKeys,
        Rosary,
        Dynamite,
        HourglassNecklace,
    }
}
