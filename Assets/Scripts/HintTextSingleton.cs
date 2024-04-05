using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class HintTextSingleton : MonoBehaviour
{
    public static HintTextSingleton Main;
    private TMP_Text actualText;

    public float hideTime = 1f;
    public float fadeTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if(Main != null)
        {
            Destroy(gameObject);
        }
        Main = this;
        actualText = GetComponentInChildren<TMP_Text>();
    }

    public void SetHintMessage(string message)
    {
        actualText.text = message; 
        if(!actualText.gameObject.activeSelf)
        {
            actualText.color = new Color(actualText.color.r, actualText.color.g, actualText.color.b, 1);
            actualText.gameObject.SetActive(true);
        }
    }

    public void HideMessage()
    {
        StopAllCoroutines();
        StartCoroutine(hideMessageRoutine());
    }

    IEnumerator hideMessageRoutine()
    {
        yield return new WaitForSeconds(hideTime);
        actualText.DOColor(new Color(actualText.color.r, actualText.color.g, actualText.color.b, 0), fadeTime);
        yield return new WaitForSeconds(fadeTime);
        actualText.gameObject.SetActive(false);
    }
}
