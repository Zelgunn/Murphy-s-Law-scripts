using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(HingeJoint))]
public class DestructiblePendulum : DestructibleTrap
{
    override protected void Awake()
    {
        base.Awake();
        Block();
    }

    public void Release()
    {
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
    }

    public void Block()
    {
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
    }
}
