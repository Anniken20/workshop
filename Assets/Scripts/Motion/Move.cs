using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Move : MonoBehaviour
{
    public float duration;
    //public float xDelta;
    //public float yDelta;
    //public float zDelta;

    public void MoveMeTo(GameObject g)
    {
        transform.DOMove(g.transform.position, duration);
    }
}
