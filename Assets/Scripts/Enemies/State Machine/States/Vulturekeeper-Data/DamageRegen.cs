using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRegen : MonoBehaviour
{
    public int dmgCutoff;
    public float regenTimer;
    [HideInInspector]
    public enum Type
    {
        enemy,
        obj
    };
    public Type type = new Type();
    public void RegenAfterDelay(int dmg){
        if(dmg < dmgCutoff){
            StartCoroutine(Regen(dmg));
        }
    }
    public IEnumerator Regen(int dmg){
        yield return new WaitForSeconds(regenTimer);
        if(type.ToString() == "x"){
            GetComponentInParent<Enemy>().currentHealth += dmg;
        }
        if(type.ToString() == "z"){
            GetComponentInParent<DamageController>().dmgTilBreak -= dmg;
        }
    }
}
