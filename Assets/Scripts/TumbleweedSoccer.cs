using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedSoccer : MonoBehaviour
{
    public BreakController moneyDropper;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            moneyDropper.BreakIntoPieces();
        }
    }
}
