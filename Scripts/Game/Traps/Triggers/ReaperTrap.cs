using UnityEngine;
using System.Collections;

public class ReaperTrap : MonoBehaviour, ITrap
{
    [SerializeField] private bool m_clockwise = true;
    //[SerializeField] [Tooltip("In seconds")] private float m_timeToStartBlade = 0.5f;
    [SerializeField] [Tooltip("In turn per second")] private float m_bladeSpeed = 1;
    [SerializeField] private DestructibleTrap m_blade;

    [Header("Sounds")]
    [SerializeField] private AudioClip m_startSound;
    [SerializeField] private AudioClip m_mainLoopSound;
    private AudioSource m_audioSource;

    private Vector3 m_basePosition;

    private bool m_launched = false;
    private bool m_starting = false;

	private void Awake ()
	{
        m_audioSource = GetComponent<AudioSource>();
        m_basePosition = m_blade.transform.position;
	}

	private void Update ()
	{
        if(!m_blade)
        {
            m_audioSource.pitch = 3;
            m_audioSource.volume /= 2;
            enabled = false;
            return;
        }

        UpdateBlade();
        UpdateAudio();
	}

    private void UpdateBlade()
    {
        m_blade.transform.position = m_basePosition;

        if (!m_launched || m_starting)
        {
            return;
        }

        if (m_clockwise)
        {
            m_blade.transform.Rotate(Vector3.up, m_bladeSpeed * Time.deltaTime * 360, Space.World);
        }
        else
        {
            m_blade.transform.Rotate(Vector3.up, m_bladeSpeed * Time.deltaTime * -360, Space.World);
        }
    }

    private void UpdateAudio()
    {
        float distanceToPlayer = (transform.position - Protector.protector.transform.position).magnitude;

        m_audioSource.pitch = 1.5f - Mathf.Min(Mathf.Log(distanceToPlayer / 20 + 1), 1) / 2;
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        if (!m_blade) return;

        m_starting = true;
        m_launched = true;

        m_audioSource.clip = m_startSound;
        m_audioSource.Play();

        StartCoroutine(StartReaper());
    }

    private IEnumerator StartReaper()
    {
        float time = 0;

        Quaternion bladeRotation = m_blade.transform.rotation;
        Quaternion targetRotation; 
        if (m_clockwise)
        {
            targetRotation = m_blade.transform.rotation * Quaternion.Euler(0, 0, -180);
        }
        else
        {
            targetRotation = m_blade.transform.rotation * Quaternion.Euler(0, 0, 180);
        }

        while (time < m_startSound.length)
        {
            if(!m_blade)
            {
                break;
            }

            float ratio = time / m_startSound.length;
            m_blade.transform.rotation = Quaternion.Slerp(bladeRotation, targetRotation, ratio * ratio);

            time += Time.deltaTime * m_audioSource.pitch;
            yield return null;
        }

        if (m_blade)
        {
            m_blade.transform.rotation = targetRotation;
        }

        m_starting = false;
        m_audioSource.clip = m_mainLoopSound;
        m_audioSource.Play();
    }

    [ContextMenu("Rearm")]
    private void RearmTimed()
    {
        
    }

    public void Rearm(float timeToRearm)
    {

    }
}
