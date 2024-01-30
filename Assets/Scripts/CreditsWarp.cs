using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsWarp : MonoBehaviour
{
    public void WarpToCredits(float time)
    {
        Invoke(nameof(Warp), time);
    }

    private void Warp()
    {
        SceneManager.LoadScene("OutroCutscene");
    }
}
