using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

/* Abstract class for modifying post processing effects at runtime.
 * 
 * 
 * 
 * 
 * Caden Henderson
 * 2023
 */

public abstract class PostProcessModifier : MonoBehaviour
{
    [SerializeField] protected float impulseFadeSpeed = 1f;
    [SerializeField] protected float transitionSpeed = 1f;
    [SerializeField] protected float startingValue = 0f;

    protected float currentValue;

    //0 by default
    //can be modified by permaChange()
    protected float permaValue = 0f;

    //volume profile reference
    protected VolumeProfile vp;

    //get reference to the bloom, chromatic, etc. override on the VolumeProfile
    public abstract void SetOverride();

    void Awake()
    {
        vp = GetComponent<Volume>().sharedProfile;
        SetOverride();
    }

    protected IEnumerator ImpulseFade()
    {
        //while not at original value
        while (!Mathf.Approximately(currentValue, permaValue))
        {
            //increment towards original value
            currentValue = Mathf.MoveTowards(currentValue, permaValue, Time.deltaTime * impulseFadeSpeed);

            //wait til next frame before executing next run through while loop
            yield return null;
        }
    }

    protected IEnumerator PermaChangeTransition()
    {
        //while not at original value
        while (!Mathf.Approximately(currentValue, permaValue))
        {
            //increment towards original value
            currentValue = Mathf.MoveTowards(currentValue, permaValue, Time.deltaTime * transitionSpeed);

            //wait til next frame before executing next run through while loop
            yield return null;
        }
    }

    public void Impulse(float targetValue)
    {
        currentValue = targetValue;

        //call looping coroutine that fades to the permaValue
        StartCoroutine(ImpulseFade());
    }

    public void PermaChange(float targetValue)
    {
        permaValue = targetValue;
        //call coroutine that transitions to the target permaValue
        StartCoroutine(PermaChangeTransition());
    }

}
