using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveShaker : MonoBehaviour
{
    public bool status;
    public float shakeSpeed;
    public float shakeAmount;
    public float shakeDuration;
    /*[SerializeField] GameObject[] graves;
    [HideInInspector]
    public enum Axis
    {
        x,
        z
    };
    public Axis axis = new Axis();*/
    //private GameObject selectedGrave;
    /*private GameObject SelectGrave()
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
        status = true;
        Debug.Log("Staring to shake");
        var selectedGrave = SelectGrave();
        if (selectedGrave != null)
        {
            Debug.Log("Selected Grave: " + selectedGrave.ToString());
            StartCoroutine(Shaking(selectedGrave));
        }
    }
    IEnumerator Shaking(GameObject grave)
    {
        float elapsedTime = 0.0f;
        var ogPos = grave.transform.position;
        while (elapsedTime < shakeDuration)
        {
            if (axis.ToString() == "X")
            {
                grave.transform.position = new Vector3(ogPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, ogPos.y, ogPos.z);
            }
            if (axis.ToString() == "Z")
            {
                grave.transform.position = new Vector3(ogPos.x, ogPos.y, ogPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        status = false;
        grave.transform.position = ogPos;
    }*/
    public void StartShaking(float sshakeSpeed, float sshakeAmount, float sshakeDuration, string axis, bool status)
    {
        if (status)
        {
            shakeSpeed = sshakeSpeed;
            shakeAmount = sshakeAmount;
            shakeDuration = sshakeDuration;
            //axis = saxis;
            StartCoroutine(Shake(shakeSpeed, shakeAmount, shakeDuration, axis));
        }
        else
        {
            
        }
    }

    IEnumerator Shake(float shakeSpeed, float shakeAmount, float shakeDuration, string axis)
    {
        float elapsedTime = 0.0f;
        var ogPos = this.transform.position;
        while (elapsedTime < shakeDuration)
        {
            Debug.Log(this.gameObject.name +": " +elapsedTime.ToString());
            if (axis.ToString() == "X")
            {
                this.transform.position = new Vector3(ogPos.x + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, ogPos.y, ogPos.z);
            }
            if (axis.ToString() == "Z")
            {
                this.transform.position = new Vector3(ogPos.x, ogPos.y, ogPos.z + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.position = ogPos;
    }

}
