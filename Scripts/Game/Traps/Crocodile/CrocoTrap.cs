using UnityEngine;
using System.Collections;

public class CrocoTrap : MonoBehaviour, ITrap
{
    [SerializeField] private ParticleSystem m_deathParticleSystemFX;
    private Animator m_animator;
    private bool m_canKill = true;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetBool("Dancer", false);
    }

    public void Trigger()
    {
        m_animator.SetTrigger("TriggerAnim");
    }

    public void Rearm(float time)
    {

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
}
