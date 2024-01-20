using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullParent : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
    }
}
