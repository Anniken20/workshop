using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    public DataManager dataManager; //Reference to Progress Data
    public int level;
    public GameObject[] levelNPCs;
    public void LoadData(GameData data){
        //Debug.Log("LOADING!!!!!");
        level = data.levelComplete;
    }
    public void SaveData(ref GameData data){}
    void Start()
    {
        dataManager.LoadGame();
        for (int i = 0; i < levelNPCs.Length; i++)
        {
            if(i == level)
                {
                    levelNPCs[i].SetActive(true);
                }
            if(i != level)
                {
                    levelNPCs[i].SetActive(false);
                }
        }
    }
    void Awake()
    {
        
    }
}
