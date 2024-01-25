using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public SaveDataHandler(string dataDirPath, string dataFileName){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(){
        string path = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(path)){
            try{
                string dataToLoad = "";
                using(FileStream stream = new FileStream(path, FileMode.Open)){
                    using(StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e){
                Debug.LogError("Error loading data from file: " +path +"\n" +e);
            }
        }
        return loadedData;
    }
    public void Save(GameData data){
        string path = Path.Combine(dataDirPath, dataFileName);
        try{
            Directory.CreateDirectory(Path.GetDirectoryName(path));
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
