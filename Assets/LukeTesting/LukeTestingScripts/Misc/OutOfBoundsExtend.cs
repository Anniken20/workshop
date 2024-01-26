using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsExtend : MonoBehaviour
{
    [SerializeField] OutOfBoundsScript parentBox;
    private void OnTriggerExit(Collider other){
            parentBox.CheckObj(other);
        }
}
