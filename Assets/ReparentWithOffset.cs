using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReparentWithOffset : MonoBehaviour
{
    [SerializeField] private Transform targetParent;
    [SerializeField] private Vector3 offset;

    public void Reparent()
    {
        //transform.SetParent(targetParent);
        //transform.localPosition = offset;
    }

}
