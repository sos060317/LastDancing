using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundPrefab;

    private PhotonView PV;

    AudioClip audioClip;

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

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void PlaySound(AudioClip sound, Vector3 pos, float pitch)
    {
        audioClip = sound;
            
        PV.RPC(nameof(RPC_PlaySound), RpcTarget.All, pos, pitch);
    }

    [PunRPC]
    private void RPC_PlaySound(Vector3 pos, float pitch)
    {
        var soundClip = Instantiate(soundPrefab, pos, Quaternion.identity);

        soundClip.gameObject.SetActive(true);

        soundClip.clip = audioClip;
        soundClip.pitch = pitch;
        soundClip.Play();

        StartCoroutine(StopSound(soundClip.gameObject, soundClip.clip.length));
    }

    IEnumerator StopSound(GameObject soundObj, float delay)
    {
        yield return YieldInstructionCache.WaitForSeconds(delay);

        if (soundObj != null)
        {
            Destroy(soundObj);
        }
    }
}