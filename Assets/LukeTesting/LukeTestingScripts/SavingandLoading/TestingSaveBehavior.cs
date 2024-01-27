using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestingSaveBehavior : MonoBehaviour, IDataPersistence
{
    public TextMeshPro text;
    public int testNum;
    public void LoadData(GameData data){
        this.testNum = data.testingSave;
    }
    public void SaveData(ref GameData data){
        data.testingSave = this.testNum;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            testNum += 1;
            this.GetComponent<Collider>().enabled = false;
        }
    }
    public void Update(){
        text.text = testNum.ToString();
    }
}
