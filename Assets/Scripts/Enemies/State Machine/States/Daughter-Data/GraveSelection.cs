using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveSelection : MonoBehaviour
{
    public bool triggered;
    [Header("Stats")]
    public int graveDamage;
    public float rotationSpeed;
    public float holdRotateDuration;
    public float shakeSpeed;
    public float shakeAmount;
    public float shakeDuration;
    public float waitTime;
    public int graveDMG;
    [HideInInspector]
    public enum Axis
    {
        x,
        z
    };
    public Axis axis = new Axis();
    [Header("References")]
    public GameObject poppyModel;
    public GameObject centerGrave;
    public GameObject poppy;
    [SerializeField] GameObject[] graves;
    private Daughter d;
    public Animator anim;
    private void Start()
    {
        //poppy.transform.position = daughterStandbyPOS.position;
        d = poppy.GetComponentInChildren<Daughter>();
        anim = d.gameObject.GetComponentInChildren<Animator>();

        //poppyModel.SetActive(false);
    }
    private GameObject SelectGrave()
    {
        
        poppyModel.SetActive(false);
        poppyModel.transform.position = d.gameObject.transform.position;
        if (graves == null || graves.Length == 0)
        {
            Debug.LogWarning("The graves list is empty or null because of the implication...");
            return null;
        }
        int randomGrave = Random.Range(0, graves.Length);
        return graves[randomGrave];
    }
    public void ActivateGrave()
    {
        var selectedGrave = SelectGrave();
        if(selectedGrave != null)
        {
            //Debug.Log("Selected Grave: " + selectedGrave.ToString());
            selectedGrave.GetComponent<GraveShaker>().StartShake(shakeSpeed, shakeAmount, shakeDuration, axis.ToString());
            selectedGrave.GetComponent<DaughterGraveWrangle>().d = poppy.GetComponent<Daughter>();
            selectedGrave.GetComponent<DaughterGraveWrangle>().atGrave = true;
        }
    }
    private void Update()
    {
        if (triggered)
        {
            ActivateGrave();
            triggered = false;
        }
    }
    public void MovePoppy(Vector3 destination)
    {
        poppyModel.SetActive(true);
        poppy.transform.position = new Vector3(destination.x, destination.y, destination.z);
    }
    public void SelectNewGrave()
    {
        foreach (var grave in graves)
        {
            grave.GetComponent<GraveShaker>().CancelPeek();
        }
        StartCoroutine(Waiting());
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Done Waiting");
        poppyModel.SetActive(false);
        ActivateGrave();
    }
    public void Whacked()
    {
        poppyModel.SetActive(false);
        SelectNewGrave();
    }
    public void PoppyCombat(GameObject grave)
    {
        //poppy.GameObject.GetComponent<NavMeshAgent>().SetActive(true);
        poppy.GetComponent<Daughter>().Shooting();
        StartCoroutine(ShootDelay());
        StartCoroutine(WrangleCooldown(grave));
    }
    IEnumerator WrangleCooldown(GameObject grave)
    {
        yield return new WaitForSeconds(GetComponentInParent<GraveSelection>().waitTime);
        Debug.Log("WrangleCooldown thing");
        Whacked();
        poppy.GetComponent<Daughter>().Idle();
        grave.GetComponent<DaughterGraveWrangle>().enabled = true;
        //poppy.GameObject.GetComponent<NavMeshAgent>().SetActive(false);
    }
    public void ReturnToCenter()
    {
        MovePoppy(centerGrave.transform.position);
        poppyModel.SetActive(true);
    }
    public void Death(){
        StopAllCoroutines();
        anim.SetBool("Idle", true);
        foreach (var grave in graves)
        {
            grave.GetComponent<GraveShaker>().CancelPeek();
        }
    }
    public IEnumerator ShootDelay(){
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("Jumping", false);
        anim.SetBool("Shooting", true);
    }
    public void RotatePoppy(){
        //poppyModel.transform.rotation = Quaternion.Euler(0, 0, 0);
        d.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
