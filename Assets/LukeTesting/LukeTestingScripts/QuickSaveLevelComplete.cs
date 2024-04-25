using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSaveLevelComplete : MonoBehaviour, IDataPersistence
{
    [SerializeField] int thisLevel;
    public DataManager dataManager;
    public void LoadData(GameData data){
    }
    public void SaveData(ref GameData data){
        data.levelComplete = this.thisLevel;
    }
    public void DeathSave(){
        if(dataManager != null)
            {
                dataManager.SaveGame();
            }
    }
    
}
