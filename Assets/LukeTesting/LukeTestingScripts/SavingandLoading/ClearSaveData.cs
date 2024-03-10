using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ClearSaveData : MonoBehaviour
{
    private DataManager dm;
    public string fileName;
    public int profiles;
    private void Start(){
        //dm = GetComponent<DataManager>();
        //fileName = dm.fileName;
    }
    public void Pressed(){
        for(int i = 1; i < profiles+1; i++){
            string filePath = Path.Combine(Application.persistentDataPath, fileName+i);
            if(File.Exists(filePath)){
                File.Delete(filePath);
                Debug.Log("Deleting file: '" +filePath +"'");
            }
            else{
                Debug.Log("Cannot Delete File: '" +filePath +"' " +"As it does not exist");
            }
        }
    }
}
