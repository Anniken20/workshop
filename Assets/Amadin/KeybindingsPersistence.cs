using UnityEngine;

public class KeybindingsPersistence : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
