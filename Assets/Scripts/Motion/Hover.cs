using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    /**
     * Makes an object hover some X and Y value forever.
     * 
     * Caden Henderson
     * 7/26/23
     * Updated 11/9/23 for z-axis and some tweaks
     * 
     */


    [SerializeField] private bool randomStart = true;
    [SerializeField] private bool hoverAutomatically = true;
    [SerializeField] private float startOffset;
    [SerializeField] private float cycleDuration;
    [SerializeField] private float xDelta;
    [SerializeField] private float yDelta;
    [SerializeField] private float zDelta;

    private bool hover;

    private float randomOffset = 0f;

    private void Start()
    {
        hover = hoverAutomatically;
        if (randomStart) randomOffset = Random.Range(0f, 1f);
    }

    private void Update()
    {
        if (!hover) return;
        float t = Mathf.Sin(Mathf.PI/cycleDuration * (Time.time + randomOffset + startOffset)) * Time.deltaTime;
        transform.localPosition += new Vector3(t * xDelta, t * yDelta, t * zDelta);
    }

    public void StopHovering()
    {
        hover = false;
    }

    public void StartHovering()
    {
        hover = true;
    }
}
