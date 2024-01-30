using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoBreakScript : MonoBehaviour, ILassoable
{
public void Lassoed(Transform attachPoint, bool active, GameObject gameObject){
    Destroy(gameObject);
 }
}
