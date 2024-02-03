using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LassoWrangle : MonoBehaviour, ILassoable
{

    public CharacterMovement iaControls;
    private InputAction lasso;
    [HideInInspector] public LassoGrappleScript player;
    [SerializeField] public bool wrangling;
    [HideInInspector] public ThirdPersonController controller;
    [SerializeField] private float barCapacity;
    [SerializeField] private float barIncrement;
    [SerializeField] private float barDepleteRate;
    public GameObject barParent;
    [SerializeField] private Image wrangleBar;
    [SerializeField] private int lossTimer;
    [HideInInspector] public Coroutine lossRoutine = null;

    private float currentAmount;
    private void Awake(){
        iaControls = new CharacterMovement();
    }
    public void Lassoed(Transform attachPoint, bool active, GameObject thisObject){
        player = FindObjectOfType<LassoGrappleScript>();
        controller = ThirdPersonController.Main;
        currentAmount = 0f;
        wrangleBar.fillAmount = 0;
        currentAmount = Mathf.Clamp(currentAmount, 0, barCapacity);
        lossRoutine = StartCoroutine(LoseTimer()); 
        StartMiniGame();

    }

    private void OnEnable(){
        lasso = iaControls.CharacterControls.Lasso;
        lasso.Enable();
    }
    private void OnDisable(){
        lasso.Disable();
    }

    private void StartMiniGame(){
        player.canLasso = false;
        controller._manipulatingLasso = true;
        wrangling = true;
        barParent.SetActive(true);
    }
    private void Update(){
        if(wrangling){
        currentAmount = Mathf.Clamp(currentAmount, 0, barCapacity);
            if(lasso.triggered ){
                currentAmount += barIncrement;
            }
            currentAmount -= barDepleteRate;
            wrangleBar.fillAmount = currentAmount / barCapacity;
            if(currentAmount >= barCapacity){
                WinMiniGame();
            }
            
        }
    }

    public virtual void WinMiniGame(){
        StartCoroutine(EnableDelay());        
    }
    public virtual void LoseMiniGame(){
        StartCoroutine(EnableDelay());        
    }

    /*private void WinMiniGame(){
        //won = true;
        StopCoroutine(lossRoutine);
        Debug.Log("U win :D");
        StartCoroutine(EnableDelay());        
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }

    private void LoseMiniGame(){
        Debug.Log("U Lose D:");
        StartCoroutine(EnableDelay());        
        wrangling = false;
        barParent.SetActive(false);
        var lassoObject = player.gameObject.GetComponent<LassoController>().projectile;
        lassoObject.GetComponent<LassoDetection>().recall = true;
    }*/

    public IEnumerator EnableDelay(){
        //Debug.Log("Timer started");
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("ENDED!!");
        player.canLasso = true;
        controller._manipulatingLasso = false;
    }

    public IEnumerator LoseTimer(){
        yield return new WaitForSeconds(lossTimer);
        //if(won == false){
        LoseMiniGame();
        //}
    }

}
