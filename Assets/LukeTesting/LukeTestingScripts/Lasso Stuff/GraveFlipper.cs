using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveFlipper : LassoWrangle
{
    private Quaternion startRotation;
    private Quaternion targetRotation;
    [SerializeField] float rotateSpeed;
    [SerializeField] float holdRotateAngle;
    private bool isRotating;
     public override void WinMiniGame()
    {
        StopCoroutine(lossRoutine);
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
        controller._manipulatingLasso = false;
        FlipGrave();
    }
    public override void LoseMiniGame()
    {
        StartCoroutine(EnableDelay());
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }
    private void FlipGrave(){
        startRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.forward*90) * startRotation;
        StartCoroutine(Rotate());
    }
    IEnumerator Rotate(){
         isRotating = true;
        while(Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        player.canLasso = true;
        transform.rotation = targetRotation;
        yield return new WaitForSeconds(holdRotateAngle);
        while (Quaternion.Angle(transform.rotation, startRotation) > 0.01f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        isRotating = false;
     
    }
}
