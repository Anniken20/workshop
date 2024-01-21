using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(){

    }
    public void Save(GameData data){
        string path = Path.Combine(dataDirPath, dataFileName);
        try{
            Directory.CreateDirectory(path.GetDirectoryName(path));
            string dataToStore = JsonUtility.ToJson(data, true);
            using(FileStream stream = new FileStream(path, FileMode.Create)){
                using(StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e){
            Debug.LogError("There was an error saving the file: " +path +"\n" +e);
        }
    }
}
