using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAudioManager : MonoBehaviour
{
    void Start()
    {
        Destroy(AudioManager.main.gameObject);
    }

}
