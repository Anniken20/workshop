using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSaveLevelComplete : MonoBehaviour, IDataPersistence
{
    [SerializeField] int thisLevel;
    public DataManager dataManager;
    private bool isDead = false;
    public void LoadData(GameData data){
    }
    public void SaveData(ref GameData data){
        if(isDead){
            data.levelComplete = this.thisLevel;
        }
    }
    public void DeathSave(){
        if(dataManager != null)
            {
                isDead = true;
                dataManager.SaveGame();
            }
    }
    
}
