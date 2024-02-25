using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour
{
    public Enemy enemyScript;
    private void OnEnable()
    {
        enemyScript.damageDelegate += TestMethod;
    }

    private void OnDisable()
    {
        enemyScript.damageDelegate -= TestMethod;
    }

    public void TestMethod()
    {
        Debug.Log("testie testicles");
    }
}
