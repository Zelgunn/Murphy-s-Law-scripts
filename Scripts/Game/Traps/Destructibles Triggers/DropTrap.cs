using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DropTrap : CrushingDestructibleTrap, ITrap
{
    [ContextMenu("Trigger")]
    public void Trigger()
    {
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
    }

    [ContextMenu("Rearm")]
    public void Rearm(float timeToRearm)
    {

    }
}
