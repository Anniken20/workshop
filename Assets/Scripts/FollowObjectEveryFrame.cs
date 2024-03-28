using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectEveryFrame : MonoBehaviour
{
    public Transform followTransform;

    private void Update()
    {
        transform.position = followTransform.position;
        transform.rotation = followTransform.rotation;
    }
}
