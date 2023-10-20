using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/* A debug-only script to toggle on/off all post processing effects
 * 
 * 
 * 
 * Caden Henderson
 * 10/19/2023
 */

public class TogglePostProcessing : MonoBehaviour
{
    public bool startOff = false;

    private void Start()
    {
        if (startOff) TogglePP();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePP();
        }
    }
    public void TogglePP()
    {
        //turn off components so they don't look for changes
        //unnecessary for debug but it's here so oh well
        GetComponent<MBloomIntensity>().enabled = !GetComponent<TogglePostProcessing>().enabled;
        GetComponent<MChromaticAberration>().enabled = !GetComponent<TogglePostProcessing>().enabled;
        GetComponent<MVignette>().enabled = !GetComponent<TogglePostProcessing>().enabled;
        
        //funny way of flip-flopping 0 -> 1 or 1 -> 0 hehehe
        GetComponent<Volume>().weight = (GetComponent<Volume>().weight + 1) % 2;
    }
}
