using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class MBloomIntensity : PostProcessModifier
{
    private UnityEngine.Rendering.Universal.Bloom _bloom;

    public override void SetOverride() {
        UnityEngine.Rendering.Universal.Bloom tmp1;
        if (vp.TryGet<UnityEngine.Rendering.Universal.Bloom>(out tmp1))
        {
            _bloom = tmp1;
            permaValue = startingValue;
            _bloom.intensity.value = permaValue;
            currentValue = permaValue;
        }
    }

    void Update()
    {
        _bloom.intensity.value = currentValue;
    }


}
