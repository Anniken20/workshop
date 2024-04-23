using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Enemy
{
    [HideInInspector] public EnemyPacingState pacingState;
    [HideInInspector] public HorseStunnedState stunnedState;
    [HideInInspector] public HorseChargeState chargeState;
    [HideInInspector] public HorseFreezeState freezeState;
    [HideInInspector] public bool isCharging;
    private Animator anim;
    private Vector3 startPOS;

    public AudioClip snortSFX;
    public AudioClip crashSFX;
    //public GameObject stunnedText;
    //public GameObject chargingText;
    private RunningParticleEmitter particles;

    [HideInInspector] public bool isStunned;
    
    private void Update()
    {
        if (isCharging == false)
            audioSource.Stop();
    }
    
    private void Awake(){

        base.MyAwake();
        startPOS = this.transform.position;
        anim = this.GetComponent<Animator>();
        idleState = gameObject.AddComponent<EnemyIdleState>();
        idleState.Initialize(this, stateMachine);

        chargeState = gameObject.AddComponent<HorseChargeState>();
        chargeState.Initialize(this, stateMachine);

        stunnedState = gameObject.AddComponent<HorseStunnedState>();
        stunnedState.Initialize(this, stateMachine);

        pacingState = gameObject.AddComponent<EnemyPacingState>();
        pacingState.Initialize(this, stateMachine);

        freezeState = gameObject.AddComponent<HorseFreezeState>();
        freezeState.Initialize(this, stateMachine);

        duelState = gameObject.AddComponent<DuelState>();
        duelState.Initialize(this, stateMachine);

        stateMachine.Initialize(idleState);
        if(transform.Find("RunningParticles") != null){
            particles = transform.Find("RunningParticles").GetComponent<RunningParticleEmitter>();
        }
    }

    public void ChargeAfterXSeconds(int x)
    {
        Invoke(nameof(Charge), x);
    }

    public void Charge(){
        //Debug.Log("bla hblah blah");
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        isCharging = true;
        stateMachine.ChangeState(chargeState);
        StartCoroutine(ChargeDelay());
        ToggleParticles(true);
        
    }
    public void StopCharge(){
        isCharging = false;
        stateMachine.ChangeState(pacingState);
        audioSource.Stop();
        ToggleParticles(false);
    }
    public void Stunned(){
        isStunned = true;
        //Debug.Log("Petah the horse is stunned");
        stateMachine.ChangeState(stunnedState);
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        audioSource.PlayOneShot(snortSFX);
        ToggleParticles(false);
    }
    public void ExitStuned(){
        isStunned = false;
        stateMachine.ChangeState(pacingState);
    }
    public void Idle(){
        if(this.animator != null) this.animator.SetBool("Idle", true);
        stateMachine.ChangeState(idleState);
        ToggleParticles(false);
    }
    public void StopIdle(){
        stateMachine.ChangeState(pacingState);
        if(this.animator != null) this.animator.SetBool("Idle", false);
    }
    public void Pacing(){
        stateMachine.ChangeState(pacingState);
    }
    public void StopPacing(){
        stateMachine.ChangeState(idleState);
    }
    public void Freeze(){
        stateMachine.ChangeState(freezeState);
        ToggleParticles(false);
    }
    public void StopFreeze(){
        stateMachine.ChangeState(idleState);
    }
    public void SetDead(){
        //if(this.animator != null) this.animator.SetBool("Dead", true);
        anim.enabled = true;
        anim.SetBool("Dead", true);
        anim.SetBool("BeingWrangled", false);
        anim.SetBool("Idle", false);
        anim.SetBool("Running", false);
        anim.SetBool("Stunned", false);
        //this.transform.position = startPOS;
        //if(this.animator != null) this.animator.SetBool("BeingWrangled", false);
        //GetComponentInChildren<HorseWrangling>().gameObject.SetActive(false);
    }

    public IEnumerator ChargeDelay()
    {
        yield return new WaitForSeconds(0.75f);
        audioSource.PlayOneShot(movingSFX);
    }
    private void ToggleParticles(bool toggle){
        if(particles != null){
            particles.ToggleParticles(toggle);
        }
    }
}
