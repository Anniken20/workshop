using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
   public int coins;
   public int levelComplete;
   public int testingSave;
   public Vector3 savedCheckpoint;
   public string checkpointScene;
   public int playerHealth;
   public List<InventoryManager.AllItems> inventoryItems;
   public List<Enemy.Enemies> deadEnemies;
   public List<string> collectedNotes;
   public int ammoCount;

//These will be the default values when the game is loaded with no save data to pull from.
   public GameData(){
        this.coins = 0;
        this.levelComplete = 0;
        this.testingSave = 0;
        this.savedCheckpoint = Vector3.zero;
        this.checkpointScene = "Tutorial 1";
        this.playerHealth = 100;
        this.inventoryItems = new List<InventoryManager.AllItems>();
        this.deadEnemies = new List<Enemy.Enemies>();
        this.collectedNotes = new List<string>();
        this.ammoCount = 0;
   }
}

