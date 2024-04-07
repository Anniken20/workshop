using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgeMist : MonoBehaviour
{
    //public List<GameObject> mistObjs = new List<GameObject>();

    public void SocialiteDeath(){
        SocialiteMist[] mistObjs = FindObjectsOfType<SocialiteMist>();
        foreach(SocialiteMist mist in mistObjs){
            mist.dead = true;
        }
        /*var socialite = FindObjectOfType<SocialiteMoveState>();
        foreach(GameObject mist in mistObjs){
            //mist.GetComponent<SocialiteMist>().FadeOut();
            mist.GetComponent<SocialiteMist>().dead = true;
            StartCoroutine(Delay());*/
        
    }
    /*private IEnumerator Delay(){
        yield return new WaitForSeconds(0.3f);
    }*/
}
