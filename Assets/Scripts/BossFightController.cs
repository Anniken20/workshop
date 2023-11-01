using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightController : MonoBehaviour
{
    public GameObject bossCharacter;

    private void Start()
    {
        // Wee full moon go weee
        MoonPhaseSystem.OnFullMoon += StartBossFight;
    }

    private void OnDestroy()
    {
        // weee full moon without issue
        MoonPhaseSystem.OnFullMoon -= StartBossFight;
    }

    private void StartBossFight()
    {
        //I am unsure how the fight should go but boss activated my trap card
        bossCharacter.SetActive(true);

    }
}
