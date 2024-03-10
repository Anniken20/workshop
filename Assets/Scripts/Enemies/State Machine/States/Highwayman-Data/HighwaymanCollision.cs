using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class HighwaymanCollision : MonoBehaviour
{
    private bool canTrigger = true;
    private Highwayman h;
    private void Start(){
        h = GetComponentInParent<Highwayman>();
    }
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player") && canTrigger){
            canTrigger = false;
            StartCoroutine(AttackDelay());
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.gameObject.CompareTag("Player") && !canTrigger){
            Debug.Log("Nevermind! =)");
            StopAllCoroutines();
            canTrigger = true;
        }
    }
    public IEnumerator AttackDelay(){
        Debug.Log("IM GONNA KILL YOU!!!!!!");
        yield return new WaitForSeconds(h.highwayData.pauseDuration);
        h.Attack();
    }
    public void FixedUpdate(){
        Vector3 player = ThirdPersonController.Main.gameObject.transform.position;
        player.y = this.transform.position.y;
        this.transform.LookAt(player);
    }
}
