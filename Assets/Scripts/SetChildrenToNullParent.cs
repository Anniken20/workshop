using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* On awake, sets all child objects to have a null parent
 * DontDestroyOnLoad objects are not allowed to be children.
 * 
 * This solves that problem so we can put all of our Core prefabs into 1.
 * 
 * 
 * Caden Henderson
 * 1/18/24
 */

public class SetChildrenToNullParent : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("child count: " + transform.childCount);
        for(int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(0).SetParent(null);
        }
    }
}
