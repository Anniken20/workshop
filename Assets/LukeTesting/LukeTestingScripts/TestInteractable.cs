using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void Interacted(){
        Debug.Log("I was Interacted with!");
    }
}
