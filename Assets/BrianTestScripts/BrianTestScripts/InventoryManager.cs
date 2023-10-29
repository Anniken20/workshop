using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    //Our Inventory items
    public List<AllItems> inventoryItems = new List<AllItems>();

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
