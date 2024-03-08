using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwaymanTrigger : MonoBehaviour
{
    private Highwayman h;
    private bool canTrigger = true;
    [SerializeField] bool pressed;
    private void Start(){
        h = GetComponentInParent<Highwayman>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canTrigger)
        {
            h.SelectTile();
            canTrigger = false;
        }
    }
    void Update()
    {
        if(pressed){
            pressed = false;
            h.SelectTile();
        }
    }
}
