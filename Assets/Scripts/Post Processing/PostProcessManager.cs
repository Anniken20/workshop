using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    /**
     * This script will modify post processing effects in a variety of ways.
     * Eventually, there will be impulse options and permanent changes. 
     * 
     * 
     * Activatee -- Modify Post Processing Values!
     * - All types of Post processing listed in Override enum
     * 
     * 
     * 7/28/2023
     */

    [Tooltip("In seconds")]
    [SerializeField] private float effectDuration;

    [SerializeField] private float value;

    [Tooltip("The effected override layer - Ex: Bloom, LensDistortion")]
    [SerializeField] private Override overrideType;

    [Tooltip("Name of the spefic attribute - Ex: intensity, x_multiplier, etc.")]
    [SerializeField] private string attribute;

    private Volume volume;
    private VolumeProfile vp;



    //this is used for impulses
    //so if i change a value from 0.5 to 1, 
    //i store the 0.5 here to know how to return to the original value
    //that was in the scene
    float bloomIntensity;

    //same thing below for other fields
    float chromaticIntensity;


    //profile settings references 
    UnityEngine.Rendering.Universal.Bloom _bloom;
    ChannelMixer _channelMixer;
    ChromaticAberration _chromaticAberration;
    ColorAdjustments _colorAdjustments;
    ColorCurves _colorCurves;
    ColorLookup _colorLookup;
    DepthOfField _depthOfField;
    FilmGrain _filmGrain;
    LensDistortion _lensDistortion;
    LiftGammaGain _liftGammaGrain;
    MotionBlur _motionBlur;
    PaniniProjection _paniniProjection;
    ShadowsMidtonesHighlights _shadowsMidtonesHighlights;
    SplitToning _splitToning;
    Tonemapping _tonemapping;
    Vignette _vignette;
    WhiteBalance _whiteBalance;

    private enum Override
    {
        Bloom,
        ChannelMixer,
        ChromaticAberration,
        ColorAdjustments,
        ColorCurves, 
        ColorLookup,
        DepthOfField,
        FilmGrain,
        LensDistortion,
        LiftGammaGain,
        MotionBlur,
        PaniniProjection,
        ShadowsMidtonesHighlights,
        SplitToning,
        Tonemapping,
        Vignette,
        WhiteBalance
    }

    private void Awake()
    {
        volume = GetComponent<Volume>();
        vp = volume.sharedProfile;
        GetAllSettingsReferences();
    }

    #region Public_Activators

    public void OnActivate()
    {
        if(overrideType == Override.Bloom)
        {
            //EditBloom();
        }          
    }

    public void BloomImpulse(float val)
    {
        bloomIntensity = _bloom.intensity.value;
        _bloom.intensity.value = val;

        StartCoroutine(BloomImpulseFade());
    }

    private IEnumerator BloomImpulseFade()
    {
        //while not at original value
        while(!Mathf.Approximately(_bloom.intensity.value, bloomIntensity))
        {
            //increment towards original value
            _bloom.intensity.value = Mathf.MoveTowards(_bloom.intensity.value, bloomIntensity, Time.deltaTime);

            //wait til next frame before executing next run through while loop
            yield return null;   
        }
        //end routine
        StopCoroutine("BloomImpulseFade");
    }

    public void ChromaticImpulse(float val)
    {
        chromaticIntensity = _chromaticAberration.intensity.value;
        _chromaticAberration.intensity.value = val;

        StartCoroutine(ChromaticImpulseFade());
    }

    private IEnumerator ChromaticImpulseFade()
    {
        //while not at original value
        while (!Mathf.Approximately(_chromaticAberration.intensity.value, chromaticIntensity))
        {
            //increment towards original value
            _chromaticAberration.intensity.value = Mathf.MoveTowards(_chromaticAberration.intensity.value,
                chromaticIntensity, Time.deltaTime);

            //wait til next frame before executing next run through while loop
            yield return null;
        }
        //end routine
        StopCoroutine("ChromaticImpulseFade");
    }

    #endregion

    private VolumeComponent GetComponentReference()
    {
        List<VolumeComponent> components = vp.components;
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i].name.Equals(overrideType.ToString()))
            {
                return components[i];
            }
        }
        return null;
          
    }

    private void GetAllSettingsReferences()
    {
        //get references for all effects

        UnityEngine.Rendering.Universal.Bloom tmp1;
        if (vp.TryGet<UnityEngine.Rendering.Universal.Bloom>(out tmp1))
        {
            _bloom = tmp1;
            _bloom.intensity.value = 0f;
        }

        ChannelMixer tmp2;
        if (vp.TryGet<ChannelMixer>(out tmp2))
        {
            _channelMixer = tmp2;
            _channelMixer.redOutGreenIn.value = 0f;
        }

        ChromaticAberration tmp3;
        if (vp.TryGet<ChromaticAberration>(out tmp3))
        {
            _chromaticAberration = tmp3;
            _chromaticAberration.intensity.value = 0f;
        }

        /*
        _bloom;
        _channelMixer;
        _chromaticAberration;


        //ABOVE THIS POINT, THE GET REFERENCES HAVE BEEN SET UP
        //HOWEVER, CHANNELMIXER DOES NOT HAVE ITS IMPULSE FUNCTION SET UP


        _colorAdjustments;
        _colorCurves;
        _colorLookup;
        _depthOfField;
        _filmGrain;
        _lensDistortion;
        _liftGammaGrain;
        _motionBlur;
        _paniniProjection;
        _shadowsMidtonesHighlights;
        _splitToning;
        _tonemapping;
        _vignette;
        _whiteBalance;*/
    }

    //debug function
    private void EditBloom()
    {
        _bloom.active = true;
        _bloom.intensity.value = 3f;
    }

    private void BloomImpulse()
    {

    }

}
