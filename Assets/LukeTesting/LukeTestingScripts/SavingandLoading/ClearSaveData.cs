using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ClearSaveData : MonoBehaviour
{
    private DataManager dm;
    public string fileName;
    public int profiles;
    public GameObject confirmationPanel;
    private int currentProfileToDelete;

    private void Start()
    {
        //dm = GetComponent<DataManager>();
        //fileName = dm.fileName;
        // Ensure confirmation panel is disabled at the start
        confirmationPanel.SetActive(false);
    }

    public void Pressed()
    {
        // Display confirmation dialog
        DisplayConfirmationDialog();
    }

    public void Individual(int profNum)
    {
        // Display confirmation dialog
        currentProfileToDelete = profNum;
        DisplayConfirmationDialog();
    }

    void DisplayConfirmationDialog()
    {
        confirmationPanel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        for (int i = 1; i < profiles + 1; i++)
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName + i);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Deleting file: '" + filePath + "'");
            }
            else
            {
                Debug.Log("Cannot Delete File: '" + filePath + "' " + "As it does not exist");
            }
        }
        // Hide confirmation panel after deletion
        confirmationPanel.SetActive(false);
    }

    public void CancelDelete()
    {
        // Hide confirmation panel without deletion
        confirmationPanel.SetActive(false);
    }
    public void DeleteIndividual(int profNum){
        string filePath = Path.Combine(Application.persistentDataPath, fileName+profNum);
        if(File.Exists(filePath)){
            File.Delete(filePath);
            Debug.Log("Deleting file: '" +filePath +"'");
        }
        else{
                Debug.Log("Cannot Delete File: '" +filePath +"' " +"As it does not exist");
        }
        confirmationPanel.SetActive(false);
    }
    
}
