using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SoundManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static SoundManager instance = null;

    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip sound, Vector3 pos, float pitch = 1)
    {
        var soundPrefab = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "SoundPrefab"), pos, Quaternion.identity)
            .GetComponent<AudioSource>();

        soundPrefab.gameObject.SetActive(true);

        soundPrefab.clip = sound;
        soundPrefab.pitch = pitch;

        StartCoroutine(StopSound(soundPrefab.gameObject, soundPrefab.clip.length));
    } 

    IEnumerator StopSound(GameObject soundObj, float delay)
    {
        yield return YieldInstructionCache.WaitForSeconds(delay);

        if (soundObj != null)
        {
            PhotonNetwork.Destroy(soundObj);
        }
    }
}