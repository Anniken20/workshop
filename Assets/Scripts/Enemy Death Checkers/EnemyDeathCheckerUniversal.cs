using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathCheckerUniversal : MonoBehaviour
{
    public GameObject enemy;
    public GameObject[] objectsToDelete;
    public GameObject[] objectsToEnable;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null && objectsToDelete != null)
        {
            for (int i = 0; i < objectsToDelete.Length; i++)
            {
                // Disable the current object
                objectsToDelete[i].SetActive(false);
            }
        }
        if (enemy == null && objectsToEnable != null)
        {
            for (int i = 0; i < objectsToDelete.Length; i++)
            {
                // Disable the current object
                objectsToEnable[i].SetActive(true);
            }
        }
    }
}
