using UnityEngine;
using System.Collections;

public class CrocoDeathTouch : MonoBehaviour
{
    [SerializeField] private CrocoTrap m_crocoTrap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Protector.protector.gameObject)
        {
            if (m_crocoTrap)
            {
                m_crocoTrap.HitProtector();
            }
            return;
        }

        if (other.gameObject == Fairy.fairy.gameObject)
        {
            if (m_crocoTrap)
            {
                m_crocoTrap.HitFairy();
            }
        }
    }
}
