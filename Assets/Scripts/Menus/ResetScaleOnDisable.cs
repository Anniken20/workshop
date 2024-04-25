using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScaleOnDisable : MonoBehaviour
{
    /*public Vector3 startSize;
    private void Start(){
        startSize = this.transform.localScale;
    }*/
    private void OnDisable(){
        this.transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnEnable(){
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
