using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgeMist : MonoBehaviour
{
    public void SocialiteDeath(){
        SocialiteMist[] mistObjs = FindObjectsOfType<SocialiteMist>();
        foreach(SocialiteMist mist in mistObjs){
            Destroy(mist.gameObject);
        }
    }
}
