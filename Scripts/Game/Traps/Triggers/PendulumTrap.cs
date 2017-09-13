using UnityEngine;
using System.Collections;

public class PendulumTrap : MonoBehaviour, ITrap
{
    [SerializeField] private DestructiblePendulum m_pendulum;
    private Vector3 m_basePendulumLocalPosition;
    private Quaternion m_basePendulumLocalRotation;

    private void Awake()
    {
        m_basePendulumLocalPosition = m_pendulum.transform.localPosition;
        m_basePendulumLocalRotation = m_pendulum.transform.localRotation;
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        m_pendulum.Release();
    }

    [ContextMenu("Rearm")]
    private void RearmTimed()
    {
        Rearm(2);
    }

    public void Rearm(float timeToRearm)
    {
        StartCoroutine(RarmCoroutine(timeToRearm));
    }

    private IEnumerator RarmCoroutine(float timeToRearm)
    {
        float time = 0;
        Vector3 localPositionWhenRearming = m_pendulum.transform.localPosition;
        Quaternion localRotationWhenRearming = m_pendulum.transform.localRotation;

        while(time < timeToRearm)
        {
            m_pendulum.transform.localPosition = Vector3.Lerp(localPositionWhenRearming, m_basePendulumLocalPosition, time / timeToRearm);
            m_pendulum.transform.localRotation = Quaternion.Slerp(localRotationWhenRearming, m_basePendulumLocalRotation, time / timeToRearm);

            time += Time.deltaTime;
            yield return null;
        }

        m_pendulum.transform.localPosition = m_basePendulumLocalPosition;
        m_pendulum.transform.localRotation = m_basePendulumLocalRotation;

        m_pendulum.Block();
    }
}
