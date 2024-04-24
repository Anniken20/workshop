using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocationPoppy : MonoBehaviour
{
    [SerializeField] GameObject followLocation;
    private bool follow = true;
 public void FixedUpdate(){
    if(followLocation != null && follow == true){
        this.transform.position = new Vector3(followLocation.transform.position.x, this.transform.position.y, followLocation.transform.position.z);
    }
 }
 public void StopFollowing(){
    follow = false;
 }
}
