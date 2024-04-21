using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScaleOnDisable : MonoBehaviour
{
    private Vector3 startSize;
    private void Start(){
        startSize = transform.localScale;
    }
    private void OnDisable(){
        this.transform.localScale = startSize;
    }
}
