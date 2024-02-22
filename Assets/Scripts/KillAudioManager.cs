using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAudioManager : MonoBehaviour
{
    void Start()
    {
        if(AudioManager.main.gameObject != null) Destroy(AudioManager.main.gameObject);
    }

}
