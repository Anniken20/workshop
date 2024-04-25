using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonFix : MonoBehaviour
{
    private void OnDisable(){
        this.transform.localScale = new Vector3(1, 1, 1);
    }
    private void OnEnable(){
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
