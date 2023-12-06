using UnityEngine;

public class SetActiveInSeconds : MonoBehaviour
{
    public GameObject obj;
    public float seconds;
    private void Start()
    {
        Invoke(nameof(Appear), seconds);
    }

    private void Appear()
    {
        obj.SetActive(true);
    }
}
