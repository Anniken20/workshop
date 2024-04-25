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
    public Animator anim;
    private bool lerptoOut = false;
    private Vector3 outPos;
    private GeneralParticleEmitter particles;
    private void Start()
    {
        ogPos = this.transform.position;
        d = GetComponentInParent<GraveSelection>().poppy.GetComponentInChildren<Daughter>();
        anim = d.gameObject.GetComponentInChildren<Animator>();
        this.outPos = this.transform.Find("OutPOS").position;
        lerptoOut = false;
        particles = this.GetComponentInChildren<GeneralParticleEmitter>();
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
        particles.ToggleParticles(true);
        while (elapsedTime < sDur)
        {
            //Debug.Log(this.gameObject.name +": " +elapsedTime.ToString());
            start = true;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        start = false;
        GetComponentInParent<GraveSelection>().MovePoppy(this.transform.Find("PeekingPOS").position);
        anim.SetBool("Idle", false);
        anim.SetBool("Climbing", true);
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
        if(this.lerptoOut){
            d.gameObject.transform.position = Vector3.Lerp(d.gameObject.transform.position, this.outPos, Time.deltaTime);
            //Debug.Log("Dist: " +Vector3.Distance(anim.gameObject.transform.position, this.outPos));
            //var outp = this.transform.Find("OutPOS");
            //Debug.Log(Vector3.Distance(d.gameObject.transform.position, this.outPos));
            if(Vector3.Distance(d.gameObject.transform.position, this.outPos) <= 0.2f){
                d.gameObject.transform.position = this.outPos;
                this.lerptoOut = false;
                //Debug.Log("OutReached");
            }
        }
    }
    
    IEnumerator PeekingWait()
    {
        yield return new WaitForSeconds(5f);
       // Debug.Log("Heading to Out POS");
        this.GetComponent<DaughterGraveWrangle>().peeking = false;
        //GetComponentInParent<GraveSelection>().MovePoppy(this.transform.Find("OutPOS").position);
        anim.SetBool("Climbing", false);
        particles.ToggleParticles(false);
        //StartCoroutine(JumpCooldown());
        anim.SetBool("Jumping", true);
        //GetComponentInParent<GraveSelection>().LerpToOutPOS(this.transform.Find("OutPOS").position);
        lerptoOut = true;
        this.GetComponent<DaughterGraveWrangle>().atGrave = false;
        //this.GetComponent<DaughterGraveWrangle>().enabled = false;
        GetComponentInParent<GraveSelection>().PoppyCombat(this.gameObject);
        //Debug.Log("Leaving");
    }
    public void CancelPeek()
    {
        particles.ToggleParticles(false);
        anim.SetBool("Idle", true);
        anim.SetBool("Climbing", false);
        StopCoroutine(PeekingWait());
        StopAllCoroutines();
    }
    /*IEnumerator JumpCooldown(){
        yield return new WaitForSeconds(4f);
        GetComponentInParent<GraveSelection>().LerpToOutPOS(this.transform.Find("OutPOS").position);

    }*/
}
