using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Key : MonoBehaviour
{
   public UnityEvent keyCollect;

   private void Start()
   {
    keyCollect.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEvents>().KeyCount);
   }

   private void OnTriggerEnter(Collider collision)
   {
    if(collision.CompareTag("Player"))
    {
        keyCollect.Invoke();
        Destroy(gameObject);
    }
   }

   public void TestMethod()
   {
    Debug.Log("Fire key collect");
   }
}
