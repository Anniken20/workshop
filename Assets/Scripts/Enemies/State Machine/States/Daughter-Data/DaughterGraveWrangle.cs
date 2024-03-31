using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaughterGraveWrangle : LassoWrangle
{
    private Quaternion startRotation;
    private float initialZ;
    private float stepZ;
    private Quaternion targetRotation;
    private bool isRotating = false;
    private float rotateSpeed;
    private float holdRotateAngle;
    [HideInInspector] public Daughter d;
    [HideInInspector] public bool atGrave;
    [HideInInspector] public bool peeking;
    private void Start()
    {
        rotateSpeed = GetComponentInParent<GraveSelection>().rotationSpeed;
        holdRotateAngle = GetComponentInParent<GraveSelection>().holdRotateDuration;
    }
    public override void WinMiniGame()
    {
        StopCoroutine(lossRoutine);
        //Debug.Log("U win :D");
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
        //player.canLasso = true;
        controller._manipulatingLasso = false;
        FlipGrave();
    }
    public override void LoseMiniGame()
    {
        //Debug.Log("U Lose D:");
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }
    private void FlipGrave()
    {
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.forward*90) * startRotation;
        StartCoroutine(Rotate());
    }
    IEnumerator Rotate()
    {
        //this.GetComponentInChildren<GraveDamageTrigger>().EnableCollider();
        if(atGrave && peeking)
        {
            d.TakeDamage(GetComponentInParent<GraveSelection>().graveDMG);
            GetComponentInParent<GraveSelection>().Whacked();
            GetComponent<GraveShaker>().CancelPeek();
            atGrave = false;
            peeking = false;
        }
        isRotating = true;
        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        player.canLasso = true;
        transform.rotation = targetRotation;
        //Enable Collider To damage poppy
        yield return new WaitForSeconds(holdRotateAngle);
        while (Quaternion.Angle(transform.rotation, startRotation) > 0.01f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        isRotating = false;
        stepZ = 0f;
        //Disable Collider
        //this.GetComponentInChildren<GraveDamageTrigger>().DisableCollider();
    }
}
