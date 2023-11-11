using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    /* Uniform scale modifier
     * 
     * 
     * 9/13/23
     * Caden Henderson
     */


    public float transitionDuration = 1f;

    [SerializeField] private AnimationCurve easing;

    private bool inTransition;
    private float timeInTransition;
    private float targetScale;
    private float oldScale;

    void Update()
    {
        Transition();
    }

    public void ScaleTo(float scale)
    {
        oldScale = transform.localScale.x;
        inTransition = true;
        targetScale = scale;
        timeInTransition = 0f;
    }

    void Transition()
    {
        if (inTransition)
        {
            timeInTransition += Time.deltaTime;
            float easeFactor = easing.Evaluate(timeInTransition / transitionDuration);
            //Debug.Log("Ease factor: " + easeFactor);
            //Debug.Log("difference: " + Mathf.Abs(currentZoom - targetCamSize));

            //ease towards target zoom
            float sc = Mathf.LerpUnclamped(oldScale, targetScale, easeFactor);
            transform.localScale = new Vector3(sc, sc, sc);

            //if basically reached end of transition duration
            if (Mathf.Abs(timeInTransition - transitionDuration) < 0.0000001f)
            {
                //snap to final scale
                transform.localScale = new Vector3(targetScale, targetScale, targetScale);

                //stop transitioning
                inTransition = false;
            }
        }
    }
}
