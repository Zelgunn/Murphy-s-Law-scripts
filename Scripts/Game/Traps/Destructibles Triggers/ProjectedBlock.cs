using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ProjectedBlock : CrushingDestructibleTrap, ITrap
{
    [SerializeField] private float m_projectionStrengh = 20;

    private Vector3 m_basePosition;
    private Quaternion m_baseRotation;

    protected override void Awake()
	{
        base.Awake();

        m_basePosition = transform.position;
        m_baseRotation = transform.rotation;

        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
	}

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;

        m_rigidbody.AddForce(transform.forward * m_projectionStrengh, ForceMode.Impulse);
    }

    [ContextMenu("Rearm")]
    public void Rearm(float timeToRearm)
    {
        transform.position = m_basePosition;
        transform.rotation = m_baseRotation;

        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
    }
}
