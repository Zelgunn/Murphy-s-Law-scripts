using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public class BatTrap : MonoBehaviour
{
    [SerializeField] private Bat m_batPrefab;
    [Header("Cage parameters")]
    [SerializeField] private int m_batCount = 3;
    [SerializeField] private Vector3 m_batsCageOrigin;
    [SerializeField] private Vector3 m_batsCageSize;
    [Header("Idle mode")]
    [SerializeField] private float m_batIdleSpeed = 10f;
    [SerializeField] private float m_batAngularSpeed = 90f;
    [Header("Attack mode")]
    [SerializeField] private float m_batAttackSpeed = 10f;

    private AudioSource m_audioSource;

    private Bat[] m_bats;
    private Bounds m_cageBounds;
    private bool m_batsAreActive = false;

    private void Awake()
    {
        m_cageBounds = new Bounds(m_batsCageOrigin, m_batsCageSize);

        m_bats = new Bat[m_batCount];

        for (int i = 0; i < m_batCount; i++)
        {
            Vector3 batPosition = GetRandomPositionInCage();

            Bat bat = Instantiate(m_batPrefab);
            bat.transform.SetParent(transform);
            bat.transform.localPosition = batPosition;
            bat.target = batPosition;
            bat.manager = this;

            m_bats[i] = bat;
        }

        m_audioSource = GetComponent<AudioSource>();
    }

    #region Idle
    public Vector3 GetRandomPositionInCage()
    {
        Vector3 randomDistribution = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        return m_batsCageOrigin + Vector3.Scale(m_batsCageSize, randomDistribution);
    }

    public Vector3 GetRandomTargetForBat(Transform bat)
    {
        if (!m_cageBounds.Contains(bat.localPosition))
        {
            return GetRandomPositionInCage();
        }

        Vector3 target = bat.localPosition;

        bool validTargetForBat = false;
        float centering = 0.75f;

        int safeGuard = 5000;

        while(!validTargetForBat)
        {
            target = GetRandomPositionInCage();
            Vector3 delta = target - m_batsCageOrigin;
            target -= delta * (1 - centering);

            Vector3 batToTarget = target - bat.localPosition;
            float dotProduct = Vector3.Dot(batToTarget, bat.forward);

            validTargetForBat = dotProduct > 0;
            centering = Mathf.Lerp(centering, 1, 0.1f);

            if(--safeGuard <= 0)
            {
                break;
            }
        }

        return target;
    }

    public float CageSpeedMultiplier(Vector3 localBatPosition)
    {
        float invSpeed = (1 - 1 / m_batIdleSpeed);

        float speedMul = 1 - Mathf.Min((localBatPosition - m_batsCageOrigin).magnitude / m_batsCageSize.magnitude, 1) * invSpeed;
        speedMul = Mathf.Pow(speedMul, 4);

        return speedMul;
    }

    public bool CageContains(Vector3 localBatPosition)
    {
        return m_cageBounds.Contains(localBatPosition);
    }

    #endregion

    #region Attack
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Protector.protector.gameObject)
        {
            if(!m_batsAreActive) ActivateBats();
        }
    }

    public void ActivateBats()
    {
        for (int i = 0; i < m_batCount; i++)
        {
            m_bats[i].mode = Bat.BatMode.Attack;
        }

        m_audioSource.Stop();

        m_batsAreActive = true;
    }

    public void DeactivateBats()
    {
        for (int i = 0; i < m_batCount; i++)
        {
            if (m_bats[i] == null) continue;
            m_bats[i].Deactivate();
            m_bats[i].ReturnToCage();
        }

        m_audioSource.Play();

        m_batsAreActive = false;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(worldBatsCageOrigin, 0.025f * transform.lossyScale.magnitude);
        Gizmos.DrawWireCube(worldBatsCageOrigin, worldBatsCageSize);
    }

    #region Get/Set
    public Vector3 worldBatsCageOrigin
    {
        get { return transform.position + Vector3.Scale(m_batsCageOrigin, transform.lossyScale); }
    }

    public Vector3 worldBatsCageSize
    {
        get { return Vector3.Scale(m_batsCageSize, transform.lossyScale); }
    }

    public float batIdleSpeed
    {
        get { return m_batIdleSpeed; }
    }

    public float batAngularIdleSpeed
    {
        get { return m_batAngularSpeed; }
    }

    public float batAttackSpeed
    {
        get { return m_batAttackSpeed; }
    }

    #endregion
}
