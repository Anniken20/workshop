using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    public string scene_name;
    public DataManager dataManager;
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(dataManager != null)
            {
                dataManager.SaveGame();
            }
            SceneManager.LoadScene(scene_name);
        }
        
    }
}
