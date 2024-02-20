using UnityEngine;
using System.Collections;

//stupid script made because i was destroying with a delay from one script that was being immediately destroyed
//meaning that it would never finish destroying after the delay

public class DestroyAfterTime : MonoBehaviour
{
    public void DestroyAfter(float t)
    {
        Destroy(gameObject, t);
    }

    public void SetInactiveAfter(float t)
    {
        StartCoroutine(SetInactiveRoutine(t));
    }

    private IEnumerator SetInactiveRoutine(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }
}
