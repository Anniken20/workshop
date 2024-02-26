using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GraveContainer;

public class GraveContainer : MonoBehaviour
{
    public GameObject[] graves;
    //public bool status;
    public float shakeSpeed;
    public float shakeAmount;
    public float shakeDuration;
    //[SerializeField] GameObject[] graves;
    [HideInInspector]
    public enum Axis
    {
        x,
        z
    };
    public Axis axis = new Axis();
    private string selectedAxis;
    private void Start()
    {
        selectedAxis = axis.ToString();
    }
    private GameObject SelectGrave()
    {
        if (graves == null || graves.Length == 0)
        {
            Debug.LogWarning("The graves list is empty or null because of the implication...");
            return null;
        }
        int randomGrave = Random.Range(0, graves.Length);
        return graves[randomGrave];
    }
    public void ShakeGrave()
    {
        var selectedGrave = SelectGrave();
        if (selectedGrave != null)
        {
            Debug.Log("Selected Grave: " + selectedGrave.ToString());
            //StartCoroutine(Shaking(selectedGrave));
            //selectedGrave.GraveShaker().Shake(shakeSpeed, shakeAmount, shakeDuration, axis.ToString());
            selectedGrave.GetComponent<GraveShaker>().StartShaking(shakeSpeed, shakeAmount, shakeDuration, selectedAxis, true);
        }
    }
}
