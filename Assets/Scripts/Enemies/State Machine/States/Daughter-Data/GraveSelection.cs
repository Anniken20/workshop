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
    [SerializeField] GameObject poppy;
    [SerializeField] GameObject[] graves;
    private void Start()
    {
        //poppy.transform.position = daughterStandbyPOS.position;
        poppyModel.SetActive(false);
    }
    private GameObject SelectGrave()
    {
        poppyModel.SetActive(false);
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
        poppy.transform.position = destination;
    }
    public void SelectNewGrave()
    {
        StartCoroutine(Waiting());
    }
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        poppyModel.SetActive(false);
        ActivateGrave();
    }
}
