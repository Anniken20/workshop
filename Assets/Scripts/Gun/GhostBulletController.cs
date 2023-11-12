using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ghost bullet system! 
 * Spawned by BulletController on destruction. 
 * Communicates with GunController for ghost ammo system
 * 
 * 
 * Caden Henderson
 * 11/11/23
 */


public class GhostBulletController : MonoBehaviour
{
    [Header("Stats")]
    public float retrievalCooldown;
    public float retrievalSpeed;
    public float retrievalAcceleration;

    [Header("Audio")]
    public AudioClip[] spawnSounds;
    public AudioClip[] retrieveSounds;

    private GameObject _player;
    private GunController _gunController;
    private float currSpeed;

    public void Spawn(GameObject player)
    {
        AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, spawnSounds[(int)Random.Range(0, spawnSounds.Length)]);
        _player = player;
        _gunController = _player.GetComponent<GunController>();
        StartCoroutine(RetrieveRoutine());
    }

    private IEnumerator RetrieveRoutine()
    {
        yield return new WaitForSeconds(retrievalCooldown);
        currSpeed = retrievalSpeed;
        while (true)
        {
            gameObject.transform.position = 
                Vector3.MoveTowards(gameObject.transform.position, 
                _player.transform.position + new Vector3(0f, 1f), 
                currSpeed * Time.deltaTime);

            //ramp up speed by some factor
            currSpeed *= 1f + (retrievalAcceleration * Time.deltaTime);

            //wait a frame before continuing loop
            yield return null;

            if (ReachedPlayer())
            {
                Despawn();

                //return to break
                yield break;
            }
        }
    }

    private bool ReachedPlayer()
    {
        return Vector3.Distance(transform.position, _player.transform.position + new Vector3(0f, 1f)) < 0.5f;
    }

    private void Despawn()
    {
        AudioManager.main.Play(AudioManager.AudioSourceChannel.SFX, retrieveSounds[0]);
        _gunController.RestoreBullet();
        Destroy(gameObject);
    }

}
