using UnityEngine;
using System.Collections;

public class FairyPowerSound : MonoBehaviour
{
    [SerializeField] private float m_pitchMultiplier = 1.5f;
    private AudioSource m_audioSource;


	private void Awake ()
	{
        m_audioSource = GetComponent<AudioSource>();
	}

    public void PlaySound(float duration)
    {
        StartCoroutine(PlaySoundCoroutine(duration));
    }

    private IEnumerator PlaySoundCoroutine(float duration)
    {
        float time = 0;
        m_audioSource.Play();

        m_audioSource.pitch = 1;

        while (time < duration)
        {
            m_audioSource.pitch += (Fairy.fairy.velocity * m_pitchMultiplier / 15) * time / duration;

            time += Time.deltaTime;
            yield return null;
        }

        m_audioSource.Stop();
    }
}
