using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    //public DataManager dataManager; //Reference to Progress Data
    public int level = 0;
    public GameObject[] levelNPCs;
    public void LoadData(GameData data){
        level = data.levelComplete;
    }
    public void SaveData(ref GameData data){}
    void Start()
    {
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
