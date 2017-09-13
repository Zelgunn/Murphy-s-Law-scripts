using UnityEngine;
using System.Collections;

public class MummyArea : MonoBehaviour
{
    [SerializeField] private Mummy m_mummy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != Protector.protector.gameObject)
        {
            return;
        }

        if(m_mummy) m_mummy.wandering = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != Protector.protector.gameObject)
        {
            return;
        }

        if (m_mummy) m_mummy.wandering = true;
    }
}
