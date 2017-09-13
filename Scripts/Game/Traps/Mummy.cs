using UnityEngine;
using System.Collections;

public class Mummy : MonoBehaviour
{
    [SerializeField] private float m_wanderingSpeed = 1;
    [SerializeField] private float m_pursuingSpeed = 2;
    [SerializeField] private float m_pursuingAcceleration = 1;
    [SerializeField] private Transform[] m_wanderingPath;
    [SerializeField] private float m_MummyDebugYOffset = 0.75f;
    [SerializeField] private ParticleSystem m_deathParticleSystemFX;
    [SerializeField] private bool m_useAnimatorSpeedBlend = true;
    private int m_wanderingIndex = 0;
    private Animator m_animator;
    private bool m_wandering = true;
    private float m_speedBlend = 0.0f;
    private int m_speedHash;
    private bool m_canKill = true;

	private void Awake ()
	{
        m_wanderingIndex = Random.Range(0, m_wanderingPath.Length);
	    m_animator = GetComponent<Animator>();
        m_speedHash = Animator.StringToHash("Speed");
	}

	private void Update ()
	{
        if (m_wandering)
        {
            UpdateWandering();
        }
        else
        {
            UpdatePursuing();
        }

        if(m_useAnimatorSpeedBlend) m_animator.SetFloat(m_speedHash, m_speedBlend);
	}

    private void UpdateWandering()
    {
        m_speedBlend = 0;

        Vector3 target = m_wanderingPath[m_wanderingIndex].position;
        target.y += m_MummyDebugYOffset;
        Vector3 deltaToTarget = target - transform.position;

        if (deltaToTarget.magnitude < 0.01f)
        {
            int currentIndex = m_wanderingIndex;
            do
            {
                m_wanderingIndex = Random.Range(0, m_wanderingPath.Length);
            }
            while (m_wanderingIndex == currentIndex);
            return;
        }

        transform.position += deltaToTarget.normalized * m_wanderingSpeed * Time.deltaTime;
        transform.LookAt(target);
    }

    private void UpdatePursuing()
    {
        if (m_speedBlend < 1)
        {
            m_speedBlend += Time.deltaTime * m_pursuingAcceleration;
        }

        if (m_speedBlend > 1)
        {
            m_speedBlend = 1;
        }

        Vector3 murphyPosition = Protector.protector.transform.position;
        murphyPosition.y += m_MummyDebugYOffset;
        Vector3 deltaToTarget = murphyPosition - transform.position;

        transform.position += deltaToTarget.normalized * (m_wanderingSpeed * (1 - m_speedBlend) + m_pursuingSpeed * m_speedBlend) * Time.deltaTime;
        transform.LookAt(murphyPosition);
    }

    public void HitProtector()
    {
        if (!m_canKill) return;
        m_canKill = false;

        Die();
        Protector.ReduceProtectorLifeCount();
    }

    public void HitFairy()
    {
        if (!m_canKill) return;
        m_canKill = false;

        Die();
    }

    private void Die()
    {
        m_deathParticleSystemFX.gameObject.SetActive(true);
        m_deathParticleSystemFX.transform.SetParent(null);

        Destroy(gameObject);
        Destroy(m_deathParticleSystemFX.gameObject, m_deathParticleSystemFX.duration);
    }

    public bool wandering
    {
        get { return m_wandering; }
        set { m_wandering = value; }
    }
}
