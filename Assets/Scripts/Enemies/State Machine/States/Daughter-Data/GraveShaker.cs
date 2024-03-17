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
    public Daughter d;
    private void Start()
    {
        ogPos = this.transform.position;
        d = GetComponentInParent<GraveSelection>().poppy.GetComponentInChildren<Daughter>();
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
        d.GetComponent<Animator>().SetBool("Idle", false);
        d.GetComponent<Animator>().SetBool("Climbing", true);
        this.GetComponent<DaughterGraveWrangle>().peeking = true;
        StartCoroutine(PeekingWait());
        //GetComponentInParent<GraveSelection>().SelectNewGrave();
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
    IEnumerator PeekingWait()
    {
        yield return new WaitForSeconds(GetComponentInParent<GraveSelection>().waitTime);
        Debug.Log("Heading to Out POS");
        this.GetComponent<DaughterGraveWrangle>().peeking = false;
        GetComponentInParent<GraveSelection>().MovePoppy(this.transform.Find("OutPOS").position);
        d.GetComponent<Animator>().SetBool("Climbing", false);
        d.GetComponent<Animator>().SetBool("Jumping", true);
        this.GetComponent<DaughterGraveWrangle>().atGrave = false;
        //this.GetComponent<DaughterGraveWrangle>().enabled = false;
        GetComponentInParent<GraveSelection>().PoppyCombat(this.gameObject);
    }
    public void CancelPeek()
    {
        StopCoroutine(PeekingWait());
        StopAllCoroutines();
    }

}
