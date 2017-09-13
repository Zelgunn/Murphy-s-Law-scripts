using UnityEngine;
using System.Collections;

public class MummyDeathTouch : MonoBehaviour
{
    [SerializeField] private Mummy m_mummy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Protector.protector.gameObject)
        {
            if (m_mummy)
            {
                m_mummy.HitProtector();
            }
            return;
        }
        
        if(other.gameObject == Fairy.fairy.gameObject)
        {
            if (m_mummy)
            {
                m_mummy.HitFairy();
            }
        }
    }
}
