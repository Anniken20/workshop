using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Settings for trigger")]
    public float orthographicZoom;
    public float duration;
    public Ease easeMode;

    [Header("If trigger is true, then the camera zoom will change on the player entering.")]
    public bool triggerEnter;

    [Header("If trigger is true, then the camera zoom will return to the scene default on the player exiting.")]
    public bool triggerExit;

    private float sceneDefaultZoom;
    private Tween camTween;

    private void Start()
    {
        sceneDefaultZoom = Camera.main.orthographicSize;

        //add collider component if needed for trigger
        if(triggerEnter || triggerExit)
        {
            if(GetComponent<Collider>() == null)
            {
                Debug.LogWarning("Added default collider to " + gameObject.name);
                Collider col = gameObject.AddComponent<BoxCollider>();
                col.isTrigger = true;
            }
        }
    }

    public void ChangeZoom(float value, float duration)
    {
        InternalChange(value, duration);
    }

    //default duration is 1 second
    public void ChangeZoom(float value)
    {
        InternalChange(value, 1f);
    }

    private void InternalChange(float value, float duration)
    {
        if (camTween != null) camTween.Kill();
        camTween = DOTween.To(() => virtualCamera.m_Lens.OrthographicSize, x => virtualCamera.m_Lens.OrthographicSize = x,
            value, duration).SetEase(easeMode);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerEnter) return;
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeZoom(orthographicZoom, duration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!triggerExit) return;
        if (other.gameObject.CompareTag("Player"))
        {
            ChangeZoom(sceneDefaultZoom, duration);
        }
    }
}
