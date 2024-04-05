using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileMenuLoading : MonoBehaviour, IDataPersistence
{
    private int coinsCollected;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] TMP_Text enemiesText;
    private int enemyCount;
    public void LoadData(GameData data){
        coinsCollected = data.coins;
        enemyCount = data.deadEnemies.Count;
        //Debug.Log("Coins from data: " +coinsCollected +"Enemies from data: " +enemyCount);
    }
    public void SaveData(ref GameData data){}

    public void UpdateText(){
        coinsText.text = "Coins Collected: " +coinsCollected;
        enemiesText.text = "Bosses Killed: " +enemyCount;
    }
}
