using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnriqueDeathChecker : MonoBehaviour
{
    public GameObject enemy;
    public GameObject self;
    public GameObject bonusTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            self.SetActive(false);
            bonusTrigger.SetActive(false);
        }
    }
}
