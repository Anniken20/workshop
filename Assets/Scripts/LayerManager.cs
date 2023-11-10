using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Singleton to manage layers so interactions are consistent without 
 * needing to setup layer masks across multiple scripts
 * 
 * 
 * Caden Henderson
 * 11/9/23
 * 
 */

public class LayerManager : MonoBehaviour
{
    [HideInInspector] public static LayerManager main;

    [Header("Bullet layers")]
    public LayerMask passThroughLayers;
    public LayerMask noRicochetLayers;
    public LayerMask gunholeLayers;
    public LayerMask shootableLayers;

    private void Start()
    {
        if(main == null)
            main = this;
    }

    private bool IsInLayer(LayerMask mask, GameObject obj)
    {
        //compare in a weird way because layer masks are bit-flags fields
        if ((mask & 1 << obj.layer) != 0)
            return true;
        else return false;
    }
    public bool IsPassThroughLayer(GameObject obj)
    {
        return IsInLayer(passThroughLayers, obj);
    }

    public bool IsNoRicochetLayer(GameObject obj)
    {
        return IsInLayer(noRicochetLayers, obj);
    }

    public bool IsGunholeLayer(GameObject obj)
    {
        return IsInLayer(gunholeLayers, obj);
    }
}
