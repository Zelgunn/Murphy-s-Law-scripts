using UnityEngine;
using System.Collections;

public class CrushingDestructibleTrap : DestructibleTrap
{
    [SerializeField] protected bool m_kinematicWhenTooSlow = true;
    protected bool m_belowThresholdVelocity = true;

    virtual protected void Update()
    {
        if (!Protector.protector) return;
        // Speed required to kill
        Vector3 deltaVelocity = Protector.protector.rigidbody.velocity - m_rigidbody.velocity;

        m_canKill = (deltaVelocity.magnitude > 1f) && (m_rigidbody.velocity.magnitude > 0.25f);

        // Kinematic if speed goes below a threashold
        bool belowThresholdVelocityNow = m_rigidbody.velocity.magnitude < 0.01f;

        if (!m_belowThresholdVelocity && belowThresholdVelocityNow)
        {
            if (m_kinematicWhenTooSlow)
            {
                rigidbody.isKinematic = true;
            }
        }

        m_belowThresholdVelocity = belowThresholdVelocityNow;
    }
}
