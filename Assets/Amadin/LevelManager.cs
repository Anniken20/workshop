using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public DataManager dataManager; //Reference to Progress Data
    public int level = 0;
    public GameObject[] levelNPCs;

    void Start()
    {
        level = DataManager.levelCompleted;
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
