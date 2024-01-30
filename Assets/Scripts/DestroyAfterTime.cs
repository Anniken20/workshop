using UnityEngine;

//stupid script made because i was destroying with a delay from one script that was being immediately destroyed
//meaning that it would never finish destroying after the delay

public class DestroyAfterTime : MonoBehaviour
{
    public void DestroyAfter(float t)
    {
        Destroy(gameObject, t);
    }
}
