using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    static private AudioManager s_singleton;
    private AudioSource m_audioSource;

    [Header("Main Screen sounds")]
    [SerializeField] private AudioClip m_clicSound;

    [Header("Game sounds")]
    [SerializeField] private AudioClip m_protectorLifeLostSound;
    [SerializeField] private AudioClip m_protectorDeathSound;
    [SerializeField] private AudioClip m_fairyDestroyingTrapSound;
    [SerializeField] private AudioClip m_fairyPowerUpSound;

    [Header("Musics")]
    [SerializeField] private AudioClip m_mainScreenMusic;
    [SerializeField] private AudioClip m_gameAmbiance;
    [SerializeField] private AudioClip m_victoryMusic;
    [SerializeField] private AudioClip m_creditsMusic;

	private void Awake ()
    {
        if (s_singleton)
        {
            Destroy(gameObject);
            return;
        }

        s_singleton = this;
        m_audioSource = GetComponent<AudioSource>();
	}

    private void _PlayMusic(AudioClip music)
    {
        if(music != null)
        {
            m_audioSource.clip = music;
        }
        m_audioSource.Play();
    }

    static public void PlayMusic(AudioClip music = null)
    {
        s_singleton._PlayMusic(music);
    }

    private void _StopMusic()
    {
        m_audioSource.Stop();
    }

    static public void StopMusic()
    {
        s_singleton._StopMusic();
    }

    static public void PlayEphemeralSoundAt(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource audioSource = new GameObject().AddComponent<AudioSource>();

        audioSource.transform.SetParent(s_singleton.transform);
        audioSource.transform.position = position;
        audioSource.spatialBlend = 1;
        audioSource.PlayOneShot(clip, volume);

        Destroy(audioSource.gameObject, clip.length);
    }

    static public void PlayOneShot(AudioClip clip, float volumeScale = 1)
    {
        s_singleton.m_audioSource.PlayOneShot(clip, volumeScale);
    }

    static public void PlayClicSound()
    {
        s_singleton.m_audioSource.PlayOneShot(s_singleton.m_clicSound, 1);
    }

    #region Sounds Access
    static public AudioClip protectorDeathSound
    {
        get { return s_singleton.m_protectorDeathSound; }
    }

    static public AudioClip protectorLifeLostSound
    {
        get { return s_singleton.m_protectorLifeLostSound; }
    }

    static public AudioClip fairyDestroyingTrapSound
    {
        get { return s_singleton.m_fairyDestroyingTrapSound; }
    }

    static public AudioClip fairyPowerUpSound
    {
        get { return s_singleton.m_fairyPowerUpSound; }
    }

    static public AudioClip mainScreenMusic
    {
        get { return s_singleton.m_mainScreenMusic; }
    }

    static public AudioClip gameAmbiance
    {
        get { return s_singleton.m_gameAmbiance; }
    }

    static public AudioClip victoryMusic
    {
        get { return s_singleton.m_victoryMusic; }
    }

    static public AudioClip creditsMusic
    {
        get { return s_singleton.m_creditsMusic; }
    }
    #endregion
}
