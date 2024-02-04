using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Provides a singleton reference to all post processing processes
 * 
 * 
 * Caden Henderson
 * 2/4/2024
 */

public class PostProcess : MonoBehaviour
{
    public static PostProcess Main;
    public MBloomIntensity _mBloomIntensity;
    public MChromaticAberration _mChromaticAberration;
    public MVignette _mMVignette;

    private void Start()
    {
        Main = this;
        _mBloomIntensity = GetComponent<MBloomIntensity>();
        _mChromaticAberration = GetComponent<MChromaticAberration>();
        _mMVignette = GetComponent<MVignette>();
    }
}
