using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveShaker : MonoBehaviour
{
    private bool start;
    private float sSpeed;
    private float sAmount;
    private float sDur;
    private string a;
    private Vector3 ogPos;
    private void Start()
    {
        ogPos = this.transform.position;
    }
    public void StartShake(float shakeSpeed, float shakeAmount, float shakeDuration, string axis)
    {
        sSpeed = shakeSpeed;
        sAmount = shakeAmount;
        sDur = shakeDuration;
        a = axis;
        StartCoroutine(Shake());
    }
    public IEnumerator Shake()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < sDur)
        {
            //Debug.Log(this.gameObject.name +": " +elapsedTime.ToString());
            start = true;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        start = false;
        GetComponentInParent<GraveSelection>().MovePoppy(this.transform.Find("PeekingPOS").position);
        GetComponentInParent<GraveSelection>().SelectNewGrave();
        this.transform.position = ogPos;
    }
    public void FixedUpdate()
    {
        if(start)
        {
            if (a.ToString() == "x")
            {
                this.transform.position = new Vector3(ogPos.x + Mathf.Sin(Time.time * sSpeed) * sAmount, ogPos.y, ogPos.z);
            }
            if (a.ToString() == "z")
            {
                this.transform.position = new Vector3(ogPos.x, ogPos.y, ogPos.z + Mathf.Sin(Time.time * sSpeed) * sAmount);
            }
        }
    }

}
