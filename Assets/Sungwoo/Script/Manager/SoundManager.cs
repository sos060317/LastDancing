using System.Collections;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class SoundManager : MonoBehaviour, IPunObservable
{
    [SerializeField] private AudioSource soundPrefab;

    private PhotonView PV;

    AudioClip audioClip;

    #region 싱글톤
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
        //audioClip = sound;

        string soundName = sound.name; // AudioClip의 이름을 전송합니다.

        PV.RPC(nameof(RPC_PlaySound), RpcTarget.All, soundName, pos, pitch);
    }

    [PunRPC]
    private void RPC_PlaySound(string soundName, Vector3 pos, float pitch)
    {
        var soundClip = Instantiate(soundPrefab, pos, Quaternion.identity);

        soundClip.gameObject.SetActive(true);

        AudioClip clip = Resources.Load<AudioClip>(soundName); // AudioClip을 이름으로 로드합니다.
        soundClip.clip = clip;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else
        {

        }
    }
}