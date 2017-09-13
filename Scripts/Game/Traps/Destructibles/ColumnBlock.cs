using UnityEngine;
using System.Collections;

public class ColumnBlock : CrushingDestructibleTrap
{
    private ColumnTrap m_column;

    protected override void OnHitProtector(Collision collision)
    {
        base.OnHitProtector(collision);

        m_column.DeactivateBlocks();
    }

    public ColumnTrap column
    {
        get { return m_column; }
        set { m_column = value; }
    }
}
