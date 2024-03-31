using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempProfileFix : MonoBehaviour
{
    public void SelectedProf(int index){
        PlayerPrefs.SetInt("profIndex", index);
    }
}
