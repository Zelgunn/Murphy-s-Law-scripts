using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DestructibleTrap : MonoBehaviour
{
    [SerializeField] [Range(0f,1f)] protected float m_fairyPowerReduction = 0.1f;
    [SerializeField] protected GameObject m_destructiblePrefab;
    [Header("Sounds")]
    [SerializeField] protected AudioClip m_onProtectorHitSound;
    [SerializeField] protected float m_onProtectorHitSoundVolume = 0.25f;
    [SerializeField] protected AudioClip m_onDestroyedSound;
    [SerializeField] protected float m_onDestroyedSoundVolume = 0.25f;
    protected Rigidbody m_rigidbody;
    protected bool m_canKill = true;
    protected bool m_active = true;
    [SerializeField] protected ParticleSystem m_deathParticleSystemFXPrefab;

    virtual protected void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    virtual public void Activate()
    {
        m_active = true;
    }

    virtual public void Deactivate()
    {
        m_active = false;
    }

    virtual protected void Destroy(Collision collision)
    {
        Fairy.fairy.destroyedElements++;
        Vector3 destructionPoint = collision.contacts[0].point;
        Vector3 normal = -collision.contacts[0].normal.normalized;

        FalconUnity.applyForce(0, normal * GameManager.falconDestructionImpulseStrength * m_fairyPowerReduction, GameManager.falconDestructionImpulseDuration);

        if(m_destructiblePrefab)
        {
            GameObject destructibleSelf = Instantiate(m_destructiblePrefab, transform.position, transform.rotation) as GameObject;
            destructibleSelf.transform.localScale = transform.lossyScale;
            Rigidbody[] rigidbodies = destructibleSelf.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody r in rigidbodies)
            {
                r.AddExplosionForce(25000, destructionPoint, 10);
            }
        }

        if(m_onDestroyedSound)
        {
            AudioManager.PlayEphemeralSoundAt(m_onDestroyedSound, collision.contacts[0].point, m_onDestroyedSoundVolume);
        }

        AudioManager.PlayOneShot(AudioManager.fairyDestroyingTrapSound);

        if(m_deathParticleSystemFXPrefab)
        {
            ParticleSystem deathParticleSystemFX = Instantiate(m_deathParticleSystemFXPrefab);

            deathParticleSystemFX.gameObject.SetActive(true);
            deathParticleSystemFX.transform.position = collision.contacts[0].point;

            Destroy(deathParticleSystemFX.gameObject, deathParticleSystemFX.duration);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_active) return;
        GameObject hitGameobject = collision.gameObject;

        if (hitGameobject == Protector.protector.gameObject)
        {
            OnHitProtector(collision);
        }
        else if (hitGameobject == Fairy.fairy.gameObject)
        {
            OnHitFairy(collision);
        }
        else if (hitGameobject.layer == LayerMask.NameToLayer("Floor"))
        {
            OnHitFloot(collision);
        }
    }

    virtual protected void OnHitProtector(Collision collision)
    {
        if (!m_canKill || !m_active)
        {
            return;
        }

        m_canKill = false;
        m_active = false;
        Protector.ReduceProtectorLifeCount();

        if (m_onProtectorHitSound)
        {
            AudioManager.PlayEphemeralSoundAt(m_onProtectorHitSound, collision.contacts[0].point, m_onProtectorHitSoundVolume);
        }
    }

    virtual protected void OnHitFairy(Collision collision)
    {
        //Fairy.fairy.DestroyElement(m_fairyPowerReduction);

        Destroy(collision);
    }

    virtual protected void OnHitFloot(Collision collision)
    {
        m_active = false;
    }

    new public Rigidbody rigidbody
    {
        get { return m_rigidbody; }
    }
}
