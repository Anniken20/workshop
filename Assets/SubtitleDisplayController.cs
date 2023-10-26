using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/* Manage dialogue inputs from multiple sources onto 1 text element
 * 
 * 
 * 
 * 
 * 
 * Caden Henderson
 * 10/26/23
 */


public class SubtitleDisplayController : MonoBehaviour
{
    private TMP_Text subtitleText;
    public float fadeTime = 0.5f;
    private int msgCount;
    private Vector3 defaultPos;

    private void Start()
    {
        subtitleText = GetComponent<TMP_Text>();
        defaultPos = gameObject.transform.position;
    }
    public void LoadMessage(string msg)
    {
        //prevent visual errors
        if (msgCount == 4) return;

        subtitleText.text += msg + "\n";
        if (msgCount == 0)
        {
            StartCoroutine(FadeTextRoutine(true));
        }
        msgCount++;
        MoveUp();
    }

    public void UnloadMessage(string msg)
    {
        //subtitleText.text = subtitleText.text.Remove(subtitleText.text.IndexOf(msg), );
        subtitleText.text = subtitleText.text.Replace(msg, "");
        subtitleText.text = subtitleText.text.TrimEnd();
        msgCount--;
        if (msgCount == 0)
        {
            StartCoroutine(FadeTextRoutine(true));
        } 
    }

    private IEnumerator FadeTextRoutine(bool turningOn)
    {
        float targetAlpha = 0f;
        if (turningOn)
        {
            targetAlpha = 1f;
            subtitleText.color = new Color(subtitleText.color.r,
                                            subtitleText.color.g,
                                            subtitleText.color.b,
                                            0f);
        }
            while (!Mathf.Approximately(subtitleText.color.a, targetAlpha))
        {
            float newA = Mathf.MoveTowards(subtitleText.color.a, targetAlpha, Time.deltaTime / fadeTime);
            subtitleText.color = new Color(subtitleText.color.r,
                                            subtitleText.color.g,
                                            subtitleText.color.b,
                                            newA);

            //wait a frame before continuing loop
            yield return null;
        }

        //clear txt
        if (msgCount == 0 && !turningOn)
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        gameObject.transform.DOMove(transform.position + new Vector3(0, 20f, 0), 0.3f).SetEase(Ease.InCubic);
    }

    private void MoveDown()
    {
        gameObject.transform.position = defaultPos;
    }
}
