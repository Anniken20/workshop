using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MVignette : PostProcessModifier
{
    private Vignette _vignette;

    public override void SetOverride() {
        Vignette tmp1;
        if (vp.TryGet(out tmp1))
        {
            _vignette = tmp1;
            permaValue = startingValue;
            _vignette.intensity.value = permaValue;
            currentValue = permaValue;
        }
    }

    void Update()
    {
        _vignette.intensity.value = currentValue;
    }


}
