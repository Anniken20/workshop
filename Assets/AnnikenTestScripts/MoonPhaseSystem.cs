using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPhaseSystem : MonoBehaviour
{
    public delegate void FullMoonEvent();
    public static event FullMoonEvent OnFullMoon;

    private void Update()
    {
        // When it's a full moon, trigger the boss event?
        if (IsFullMoon())
        {
            OnFullMoon?.Invoke();
        }
    }

    private bool IsFullMoon()
    {
        // when is it full moon???
        // You might need data for moon phases or use a time-based approach we can do seconds or minute based?
        // Return true if it's a full moon, otherwise return false
        return false; 
    }
}
