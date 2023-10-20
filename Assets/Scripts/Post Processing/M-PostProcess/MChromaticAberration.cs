using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MChromaticAberration : PostProcessModifier
{
    private ChromaticAberration _chromaticAberration;

    public override void SetOverride() {
        ChromaticAberration tmp1;
        if (vp.TryGet(out tmp1))
        {
            _chromaticAberration = tmp1;
            permaValue = startingValue;
            _chromaticAberration.intensity.value  = permaValue;
            currentValue = permaValue;
        }
    }

    void Update()
    {
        _chromaticAberration.intensity.value = currentValue;
    }


}
