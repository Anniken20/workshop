using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
   public int coins;
   public int levelComplete;
//These will be the default values when the game is loaded with no save data to pull from.
   public GameData(){
        this.coins = 0;
        this.levelComplete = 0;
   } 
}
