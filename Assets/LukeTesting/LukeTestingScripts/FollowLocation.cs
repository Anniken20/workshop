using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocation : MonoBehaviour
{
    [SerializeField] GameObject followLocation;
    private bool follow = true;
 public void FixedUpdate(){
    if(followLocation != null && follow == true){
        this.transform.position = followLocation.transform.position;
    }
 }
 public void StopFollowing(){
    follow = false;
 }
}
