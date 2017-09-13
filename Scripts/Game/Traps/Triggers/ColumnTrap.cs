using UnityEngine;
using System.Collections;

public class ColumnTrap : MonoBehaviour, ITrap
{
    [SerializeField] private float m_massMultiplicator = 5;
    private ColumnBlock[] m_heavyBlocks;

    private void Awake()
    {
        m_heavyBlocks = GetComponentsInChildren<ColumnBlock>();
    }

    private void Start()
    {
        foreach (ColumnBlock heavyBlock in m_heavyBlocks)
        {
            heavyBlock.rigidbody.isKinematic = true;
            heavyBlock.column = this;
        }
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        foreach (ColumnBlock heavyBlock in m_heavyBlocks)
        {
            heavyBlock.rigidbody.mass *= m_massMultiplicator;
            heavyBlock.rigidbody.isKinematic = false;
        }
    }

    public void Rearm(float timeToRearm)
    {

    }

    public void DeactivateBlocks()
    {
        foreach (ColumnBlock heavyBlock in m_heavyBlocks)
        {
            heavyBlock.Deactivate();
        }
    }
}
